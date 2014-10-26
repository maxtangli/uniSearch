using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PokerGallaryController : MonoBehaviour {
	public UISearcher uiSearcher;
	CardDataProvider provider = new CardDataProvider();

	// for updateView
	public UIGrid uiGrid;
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

	void onDataFetched (DataProviderResult<Card> result)
	{
		uiSearcher.NumTotal = result.numFiltered;
		// buggy code: InstantiatePrefab executed unintended multiple times.
		//  Func<Card, GameObject> dataToPrefab = card => UnityUtils.InstantiatePrefab(uiCardImagePrefab, p => p.Card = card).gameObject;
		//  IEnumerable<GameObject> prefabs = result.datas.Select (dataToPrefab);
		//var prefabs = result.datas.Select (card => UICardImage.InstantiatePrefab (card)).ToList ();
		IList<GameObject> prefabs = new List<GameObject> ();
		foreach (var card in result.datas) {
			prefabs.Add(UICardImage.InstantiatePrefab(card).gameObject);
		}
		uiGrid.setContents(prefabs);

		watch.Stop ();
		Debug.Log ("Data fetch and set cost " + watch.ElapsedMilliseconds + " ms.");
		watch.Reset ();
	}
}


