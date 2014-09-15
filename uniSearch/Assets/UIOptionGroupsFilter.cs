using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Implement UIFilter by a mapping between OptionGroups and UIOptionGroups.
public class UIOptionGroupsFilter : UIFilter {
	#region implemented abstract members of UIFilter

	[SerializeField]
	List<OptionGroup> optionGroups = new List<OptionGroup>();
	public override IEnumerable<OptionGroup> OptionGroups {
		get { 
			return optionGroups;
		}
		set {
			optionGroups = value.ToList();
			loadUIs();
		}
	}

	#endregion

	[ContextMenu("loadUIs")]
	void loadUIs() {
		foreach (var optionGroup in OptionGroups) {
			var ui = getUI(optionGroup);
			ui.OptionGroup = optionGroup;
			ui.Interaction += onMemberInteraction;
		}
	}

	UIOptionGroup getUI(OptionGroup g) {
		return getUIByName (g.Title);
	}

	UIOptionGroup getUIByName(string name) {
		return GetComponentsInChildren<UIOptionGroup> ().Where (x => x.name == name).FirstOrDefault ();
	}

	void onMemberInteraction(object ui, EventArgs g) {

		OnInteraction ();
	}
}

// Manage OptionGroup.

// A Null UIOptionGroup which provide debug operations in inspector. 
