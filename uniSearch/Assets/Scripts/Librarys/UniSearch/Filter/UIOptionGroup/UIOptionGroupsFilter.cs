using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Implement UIFilter by a mapping between List<OptionGroup> and List<UIOptionGroup>.
// FilterData.set : 
public class UIOptionGroupsFilter : UIFilter {
		
	#region implemented abstract members of UIFilter

	FilterData filterData;
	public override FilterData FilterData {
		get { 
			return filterData;
		}
		set {
			filterData = value;
			syncViewWithFilterData();
		}
	}

	#endregion

	public virtual void syncViewWithFilterData ()
	{
		if (FilterData.Count () != uiOptionGroups.Count ()) {
			throw new Exception(
				string.Format("uiOptionGroups count error. Expected:[{0}], actual:[{1}]", 
			              FilterData.Count(), uiOptionGroups.Count ())
				);
		}

		foreach (int i in Enumerable.Range(0, uiOptionGroups.Count())) {
			uiOptionGroups.ElementAt(i).OptionGroupData = FilterData.ElementAt(i);
		}
	}

	List<UIOptionGroup> uiOptionGroups;
	void Awake() {
		uiOptionGroups = GetComponentsInChildren<UIOptionGroup> ().ToList();
		foreach (var uiOptionGroup in uiOptionGroups) {
			uiOptionGroup.Interaction += onUIOptionGroup;
		}
	}

	void onUIOptionGroup(object sender, EventArgs arg) {
		var uiOptionGroup = sender as UIOptionGroup;
		FilterData = FilterData.SpecificCheck (uiOptionGroup.OptionGroupData);
		OnInteraction ();
	}
}