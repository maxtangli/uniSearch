using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class UIButtonOptionGroup : UIOptionGroup {
	#region implemented abstract members of UIOptionGroup

	[SerializeField]
	OptionGroup optionGroup;
	public override OptionGroup OptionGroup {
		get {
			return optionGroup;
		}
		set {
			optionGroup = value;
			updateView();
		}
	}

	#endregion

	void updateView ()
	{
		int i = 0;
		foreach (var option in OptionGroup.Options) {
			var button = buttons[i++];
			button.GetComponentInChildren<UILabel>().text = option.OptionName;
			button.isEnabled = option.Selected;
		}
	}

	public List<UIButton> buttons;

	
	void Awake() {
		foreach (var button in buttons) {
			EventDelegate.Add(button.onClick, onButton);
		}
	}

	void onButton() {
		var clickedButton = UIButton.current;
		setUnique (clickedButton);
		OnInteraction ();
	}

	void setUnique(UIButton unique) {
		foreach (var button in buttons) {
			button.isEnabled = (button.Equals(unique) ? false : true);
		}
	}
}
