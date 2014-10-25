using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Example: adapt a NGUIButton with a UILabel child and a UISprite into UIOption
public class NGUIButtonOptionAdaptor : UIOption {
	#region implemented abstract members of UIOption

	UIButton button;
	UILabel label;

	OptionData optionData;
	public override OptionData OptionData {
		get {
			return optionData;
		}
		set {
			optionData = value;
			label.text = string.Format("{0}{1}", value.IsChecked ? "+":"-", value.Name);
		}
	}
	
	void Awake() {
		button = GetComponent<UIButton> ();
		label = GetComponentInChildren<UILabel> ();
		EventDelegate.Add(button.onClick, onButtonClick);
	}

	void onButtonClick() {
		OptionData = OptionData.AntiCheck();
		OnInteraction ();
	}

	#endregion

}