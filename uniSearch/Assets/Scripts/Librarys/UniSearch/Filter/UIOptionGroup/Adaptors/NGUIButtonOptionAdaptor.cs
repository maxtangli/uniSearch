using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Manage OptionGroup.

// adapt a NGUIButton with a UILabel child into UIOption
public class NGUIButtonOptionAdaptor : UIOption {
	#region implemented abstract members of UIOption

	UIButton button;
	UILabel label;

	public override Option Option {
		get {
			return new Option(label.text, !button.isEnabled);
		}
		set {
			label.text = value.Name;
			button.isEnabled = !value.Selected;
		}
	}
	
	void Awake() {
		button = GetComponent<UIButton> ();
		label = GetComponentInChildren<UILabel> ();
		EventDelegate.Add(button.onClick, onButtonClick);
	}

	void onButtonClick() {
		Option = Option.select ();
		OnInteraction ();
	}

	#endregion

}