using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Implement UIFilter by a mapping between List<OptionGroup> and List<UIOptionGroup>.
public class UIOptionGroupsFilter : UIFilter {
		
	#region implemented abstract members of UIFilter
	
	public override FilterData FilterData {
		get { 
			return new FilterData(uis.Select(x => x.OptionGroup));
		}
		set {
			foreach (var g in value) {
				var ui = getUI(g);
				ui.OptionGroup = g;
				ui.Interaction += onMemberInteraction;
			}
		}
	}

	#endregion

	IEnumerable<UIOptionGroup> uis {
		get {
			return GetComponentsInChildren<UIOptionGroup>();
		}
	}

	UIOptionGroup getUI(OptionGroup g) {
		return getUIByName (g.Title);
	}

	UIOptionGroup getUIByName(string name) {
		return GetComponentsInChildren<UIOptionGroup> ().Where (x => x.name == name).First();
	}
		
	void onMemberInteraction(object uiObj, EventArgs arg) {
		OnInteraction ();
	}
}