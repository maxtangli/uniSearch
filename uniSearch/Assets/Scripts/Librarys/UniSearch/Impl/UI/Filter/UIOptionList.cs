using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// implement UIOptionGroup by a list of UIOption
public class UIOptionList : UIOptionGroup {

	[SerializeField]
	OptionGroupData optionGroupData;
	public override OptionGroupData OptionGroupData {
		get {
			return optionGroupData;
		}
		set {
			optionGroupData = value;
			sycnViewWithOptionGroup();
		}
	}

	protected virtual void sycnViewWithOptionGroup ()
	{
		if (OptionGroupData.Count() != uiOptions.Count ()) {
			throw new Exception(
				string.Format("uiOptions count error. Expected:[{0}], actual:[{1}]", 
			              OptionGroupData.Count(), uiOptions.Count ())
			);
		}
		foreach (int i in Enumerable.Range(0, uiOptions.Count())) {
			uiOptions.ElementAt(i).OptionData = OptionGroupData.ElementAt(i);
		}
	}

	[SerializeField]
	List<UIOption> uiOptions; // mapping by order
	void Awake() {
		uiOptions = GetComponentsInChildren<UIOption> ().ToList ();
		foreach (var ui in uiOptions) {
			ui.Interaction += onUIOption;
		}
	}

	void onUIOption(object sender, EventArgs e) {
		var ui = sender as UIOption;
		OptionGroupData = OptionGroupData.SpecificCheck (ui.OptionData.IsChecked, ui.OptionData.Name);
		OnInteraction ();
	}
}