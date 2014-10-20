using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class PokerGallaryController : MonoBehaviour {

	public UISearcher uiSearcher;
	CardDataProvider provider = new CardDataProvider();
	public UIGrid uiGrid;
	public UICardImage uiCardImagePrefab;
	System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
	void Start() {
		uiSearcher = uiSearcher ?? GetComponentInChildren<UISearcher> ();
		uiSearcher.Interaction += onUISearch;

		uiSearcher.SearcherData = provider.SearcherCandidate;
		uiSearcher.OnInteraction ();
	}

	[ContextMenu("initUISearcher")]
	void initUISearcher() {
		uiSearcher.SearcherData = provider.SearcherCandidate;
	}
	
	void onUISearch(object sender, EventArgs e) {
		watch.Start ();
		provider.fetch (uiSearcher.SearcherData, onDataFetched);
	}

	void onDataFetched (DataProvider<Card>.Result result)
	{
		Debug.Log("onDataFetched");
		uiSearcher.numTotal = result.numFiltered;
		// buggy code: InstantiatePrefab executed unintended multiple times.
		//  Func<Card, GameObject> dataToPrefab = card => UnityUtils.InstantiatePrefab(uiCardImagePrefab, p => p.Card = card).gameObject;
		//  IEnumerable<GameObject> prefabs = result.datas.Select (dataToPrefab);
		IList<GameObject> prefabs = new List<GameObject> ();
		foreach (var card in result.datas) {
			prefabs.Add(
				UnityUtils.InstantiatePrefab(uiCardImagePrefab, p => p.Card = card).gameObject);
		}
		uiGrid.setContents(prefabs);
		watch.Stop ();
		Debug.Log (watch.ElapsedMilliseconds);
		watch.Reset ();
	}	
}

public interface IFilter<T> {
	bool match (T obj, OptionGroupData condition);
	OptionGroupData Candidate {get;}

}

public interface IFilterChain<T> {
	bool match (T obj, IEnumerable<OptionGroupData> conditions);
	IEnumerable<OptionGroupData> Candidates {get;}
}

public class PredicateFilter<T> : IFilter<T> {
	IDictionary<string, Predicate<T>> optionNameToPredicates;
	OptionGroupData candidate;
	public PredicateFilter(OptionGroupData candidate, IDictionary<string, Predicate<T>> optionNameToPredicates) {
		System.Diagnostics.Debug.Assert (candidate.Count () == optionNameToPredicates.Count);
		this.candidate = candidate;
		this.optionNameToPredicates = optionNameToPredicates;
	}
	
	#region IFilter implementation
	
	public bool match (T obj, OptionGroupData condition)
	{
		return condition.Any(x => getPredicate (x.Name) (obj));
	}

	public OptionGroupData Candidate {
		get {
			return candidate;
		}
	}
	
	#endregion
	
	Predicate<T> getPredicate(string optionName) {
		return optionNameToPredicates[optionName];
	}
}

public class FilterChain<T> : IFilterChain<T> {
	IEnumerable<IFilter<T>> filters;
	
	public FilterChain(IEnumerable<IFilter<T>> filters) {
		this.filters = filters;
	}
	
	#region IFilterChain implementation
	
	public bool match (T obj, IEnumerable<OptionGroupData> conditions)
	{
		return conditions.All (x => getFilter (x).match (obj, x));
	}

	public IEnumerable<OptionGroupData> Candidates {
		get {
			return filters.Select(x => x.Candidate);
		}
	}

	#endregion
	
	IFilter<T> getFilter(OptionGroupData condition) {
		return filters.Where (x => x.Candidate.SameCandidate (condition)).Single ();
	}
}

// Provide data under search conditions.
// NOTE: multi-thread not supported
// TODO: generic base class not so good?
public abstract class DataProvider<T> {
	public class Result
	{
		public SearcherData ConditionSearcherData { get; protected set;}
		public int numFiltered  { get; protected set;}
		public int numRangered {
			get {
				return datas.Count();
			}
		}
		public IEnumerable<T> datas{ get; protected set;}
		public Result (SearcherData conditionSearcherData, int numFiltered, IEnumerable<T> datas)
		{
			this.ConditionSearcherData = conditionSearcherData;
			this.numFiltered = numFiltered;
			this.datas = datas;
		}
		public override string ToString ()
		{
			return string.Format ("{0},{1},{2},{3}", 
			                     ConditionSearcherData.ToString (),
			                     numFiltered.ToString (), numRangered.ToString (),
			                     datas.ToString ());
		}
	}
	public abstract SearcherData SearcherCandidate { get;}
	public abstract void fetch(SearcherData searcherConditions, Action<Result> onDataFetched);
}

public class CardDataProvider : DataProvider<Card> {

	IFilterChain<Card> filterChain;
	SearcherData searchCandidate;

	public CardDataProvider() {
		OptionGroupData colorOptionGroup = new OptionGroupData("COLOR",OptionData.CreateMany("HEART","SPADE","CLUB","DIAMOND"));
		IEnumerable<OptionGroupData> optionGroups = new List<OptionGroupData> () {
			colorOptionGroup
		};
		searchCandidate = new SearcherData (new FilterData (optionGroups), new SorterData (), new RangerData ());

		IFilter<Card> colorFilter = new PredicateFilter<Card> (
			colorOptionGroup, 
		    new Dictionary<string, Predicate<Card>> () {
				{"HEART", x => x.Color == CardColor.HEART},
				{"SPADE", x => x.Color == CardColor.SPADE},
				{"CLUB", x => x.Color == CardColor.CLUB},
				{"DIAMOND", x => x.Color == CardColor.DIAMOND},
			}
		);
		filterChain = new FilterChain<Card> (
			new IFilter<Card>[] {colorFilter}
		);
	}

	#region implemented abstract members of DataProvider

	public override void fetch (SearcherData searcherCondition, Action<Result> onDataFetched)
	{
		// get full datas
		var allCards = Enumerable.Range (1, 52).Select (
			x => new Card ( CardUtil.CodeToCardColor(x), CardUtil.CodeToPoint(x)) );

		// handle search
		var filteredCards = 
				from card in allCards 
				where filterChain.match (card, searcherCondition.FilterData) // TODO support IQuerable
				orderby card.Code ascending
				select card;

		int numTotal = filteredCards.Count();

		var rangeredCards = filteredCards.Skip (searcherCondition.RangerData.Index).Take (searcherCondition.RangerData.Count);
		
		var result = new Result (searcherCondition,
		                         numTotal,
		                         rangeredCards);

		Debug.Log (result);
		// finish
		onDataFetched (result);
	}

	public override SearcherData SearcherCandidate {
		get {
			return searchCandidate;
		}
	}

	#endregion
}


public static class IEnumerableExtensions {
	// By default, MonoDevelop's configuration not support IEnumerable.Zip which comes from .net 4.0.
	// TODO configure issues.
	// http://stackoverflow.com/questions/16927845/cs1061-cs0117-system-linq-enumerable-does-not-contain-a-definition-for-zip
	public static IEnumerable<T> Zip<A, B, T>(
		this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
	{
		if (seqA == null) throw new ArgumentNullException("seqA");
		if (seqB == null) throw new ArgumentNullException("seqB");
		
		using (var iteratorA = seqA.GetEnumerator())
			using (var iteratorB = seqB.GetEnumerator())
		{
			while (iteratorA.MoveNext() && iteratorB.MoveNext())
			{
				yield return func(iteratorA.Current, iteratorB.Current);
			}
		}
	}
}