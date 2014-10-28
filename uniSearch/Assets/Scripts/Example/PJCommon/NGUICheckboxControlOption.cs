using UnityEngine;
using System.Collections;

public class NGUICheckboxControlOption : UIOption {
	#region implemented abstract members of UIOption
	OptionData optionData;
	public override OptionData OptionData {
		get {
			return optionData;
		}
		set {
			optionData = value;
			updateViewWithOptionData();
		}
	}
	#endregion

	void updateViewWithOptionData ()
	{
		uiLabel.text = OptionData.Name;
		enableUIToggleChange = false;
		uiToggle.value = OptionData.IsChecked;
		enableUIToggleChange = true;
	}

	UIToggle uiToggle;
	UILabel uiLabel;
	void Awake() {
		uiToggle = GetComponent<UIToggle> ();
		uiLabel = transform.FindChild ("Label").GetComponent<UILabel> ();
		EventDelegate.Add (uiToggle.onChange, onUIToggleChange);
	}

	bool enableUIToggleChange = true;
	void onUIToggleChange() {
		if (enableUIToggleChange) {
			if (OptionData != null) { // avoid UIToggle.Start
				OptionData = OptionData.AntiCheck();
				OnInteraction();
			}
		}
	}
}