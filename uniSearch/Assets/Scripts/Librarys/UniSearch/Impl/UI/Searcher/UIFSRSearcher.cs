using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// implement UISearcher with a UIFilter, UISorter and UIRanger
[ExecuteInEditMode]
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

	public override int NumTotal {
		get {
			return uiRanger.NumTotal;
		}
		set {
			uiRanger.NumTotal = value;
		}
	}
	#endregion

	[ContextMenu("initUISearcher")]
	void Awake() {
		uiFilter = uiFilter ?? GetComponentInChildren<UIFilter> () ?? gameObject.AddComponent<UINullFilter>();
		uiSorter = uiSorter ?? GetComponentInChildren<UISorter> () ?? gameObject.AddComponent<UINullSorter>();
		uiRanger = uiRanger ?? GetComponentInChildren<UIRanger> () ?? gameObject.AddComponent<UINullRanger>();
		
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