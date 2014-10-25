using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Provide data under search conditions.
// WARNING: multi-thread not supported
public interface IDataProvider<T> {
	SearcherData SearcherCandidate { get;}
	void fetch(SearcherData searcherConditions, Action<DataProviderResult<T>> onDataFetched);
}

public class DataProviderResult<T>
{
	public SearcherData ConditionSearcherData { get; protected set;}
	public int numFiltered  { get; protected set;}
	public virtual int numRangered {get { return datas.Count();}}
	public IEnumerable<T> datas{ get; protected set;}
	public DataProviderResult (SearcherData conditionSearcherData, int numFiltered, IEnumerable<T> datas)
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