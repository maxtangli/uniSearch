using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// implement UISearcher with a UIFilter, UISorter and UIRanger
public class UIFSRSearcher : UISearcher {
	public UIFilter uiFilter;
	public UISorter uiSorter;
	public UIRanger uiRanger;

	#region implemented abstract members of UISearcher
	public override SearcherData SearcherData {
		get {
			return new SearcherData(uiFilter.FilterData, uiSorter.SorterData, uiRanger.RangerData);
		}
		set {
			uiFilter.FilterData = value.FilterData;
			uiSorter.SorterData = value.SorterData;
			uiRanger.RangerData = value.RangerData;
		}
	}

	public override int numTotal {
		get {
			return uiRanger.numTotal;
		}
		set {
			uiRanger.numTotal = value;
		}
	}
	#endregion

	void Awake() {
		uiFilter = uiFilter ?? GetComponentInChildren<UIFilter> ();
		uiSorter = uiSorter ?? GetComponentInChildren<UISorter> ();
		uiRanger = uiRanger ?? GetComponentInChildren<UIRanger> ();
		
		uiFilter.Interaction += onUIFilter;
		uiSorter.Interaction += onUISorter;
		uiRanger.Interaction += onUIRanger;
	}

	void onUIFilter(object sender, EventArgs e) {
		uiRanger.RangerData = uiRanger.DefaultRangerData;
		OnInteraction ();
	}

	void onUISorter (object sender, EventArgs e)
	{
		uiRanger.RangerData = uiRanger.DefaultRangerData;
		OnInteraction ();
	}

	void onUIRanger (object sender, EventArgs e)
	{
		OnInteraction ();
	}
}