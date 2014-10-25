using UnityEngine;
using System.Collections;

// Immutable data for searcher.
public class SearcherData {
	FilterData filterData;
	public FilterData FilterData {
		get {
			return filterData;
		}
	}

	SorterData sorterData;
	public SorterData SorterData {
		get {
			return sorterData;
		}
	}

	RangerData rangerData;
	public RangerData RangerData {
		get {
			return rangerData;
		}
	}

	public SearcherData (FilterData filterData, SorterData sorterData, RangerData rangerData)
	{
		this.filterData = filterData;
		this.sorterData = sorterData;
		this.rangerData = rangerData;
	}

	public override string ToString ()
	{
		return string.Format ("[{0},{1},{2}]", FilterData, SorterData, RangerData);
	}
}
