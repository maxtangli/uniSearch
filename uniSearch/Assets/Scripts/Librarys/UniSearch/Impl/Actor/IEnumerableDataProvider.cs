using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class IEnumerableDataProvider<T> : IDataProvider<T> {
	IEnumerable<T> fullDatas;
	IFilter<T> filter;
	SearcherData searchCandidate;
	// TODO sorter support
	public IEnumerableDataProvider(IEnumerable<T> fullDatas, IFilter<T> filter) {
		this.fullDatas = fullDatas;
		this.filter = filter;
		searchCandidate = new SearcherData (filter.Candidates, new SorterData (), new RangerData ());
	}

	public void fetch (SearcherData searcherCondition, Action<DataProviderResult<T>> onDataFetched)
	{
		Debug.Log("fetch condition: " + searcherCondition);
		var filteredDatas = 
			from data in fullDatas 
			where filter.match (data, searcherCondition.FilterData) // TODO support IQuerable
			//orderby data ascending // TODO sorter
			select data;
		int numFiltered = filteredDatas.Count();
		var rangeredDatas = filteredDatas.Skip (searcherCondition.RangerData.Index).Take (searcherCondition.RangerData.Count);
		var result = new DataProviderResult<T> (searcherCondition, numFiltered, rangeredDatas);

		Debug.Log ("fetch result: " + result);
		onDataFetched (result);
	}

	public SearcherData SearcherCandidate {
		get {
			return searchCandidate;
		}
	}
}
