using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// A card data provider to support data querying by UI.
/* Note these usecases
1. one dataProvider may be reused by several scenes.
For example, in a card game, once a UserCardDataProvider is implemented, 
it can be used to support querying in card overview / deck edit / friend owned cards / etc.

2. One dataProvider can be composited by serveral other data providers.
I'm sorry by now I do not come up with any examples for this :(

*/
public class CardDataProvider : IDataProvider<Card> {
	IEnumerableDataProvider<Card> provider;
	public CardDataProvider() {
		// Local data querying, which is the most common case in social-game querying,
		// can be easily implemented by reusing IEnumerableDataProvider with 4 step:

		// 1. provide a lsit of full cards.
		var allCards = Enumerable.Range (1, 52).Select (
			x => new Card ( CardUtil.CodeToCardColor(x), CardUtil.CodeToPoint(x)) );

		// 2. provider a IFilter.
		//    which can be eaisy made by using PredicatesFilter with a declartion of filter options info.
		IFilter<Card> filter = new PredicatesFilter<Card> (
			new Dictionary<string, IDictionary<string, Predicate<Card>>>() {
				{
					"COLOR" ,new Dictionary<string, Predicate<Card>> () {
						{"HEART", x => x.Color == CardColor.HEART},
						{"SPADE", x => x.Color == CardColor.SPADE},
						{"CLUB", x => x.Color == CardColor.CLUB},
						{"DIAMOND", x => x.Color == CardColor.DIAMOND},
					}
				},
				{
					"POINT" ,new Dictionary<string, Predicate<Card>> () {
						{"A", x => x.Point == 1},
						{"2-5", x => x.Point >= 2 && x.Point <= 5},
						{"6-10", x => x.Point >= 6 && x.Point <= 10},
						{"J-K", x => x.Point >= 11},
					}
				}
			}
		);

		// 3. new a dataProvider with a list of full datas IEnumerable and a IFilter.
		provider = new IEnumerableDataProvider<Card> (allCards, filter);
	}

	// 4. delegate IDataProvider operations to IEnumerableDataProvider
	#region implemented abstract members of DataProvider

	public void fetch (SearcherData searcherCondition, Action<DataProviderResult<Card>> onDataFetched)
	{
		provider.fetch (searcherCondition, onDataFetched);
	}

	public SearcherData SearcherCandidate {
		get {
			return provider.SearcherCandidate;
		}
	}

	#endregion
}