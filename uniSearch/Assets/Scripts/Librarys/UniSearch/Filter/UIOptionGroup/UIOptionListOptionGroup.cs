using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// implement UIOptionGroup by a list of UIOption
public class UIOptionListOptionGroup : UIOptionGroup {
	public bool onlyOneSelected = false;

	[SerializeField]
	OptionGroup optionGroup;
	public override OptionGroup OptionGroup {
		get {
			return optionGroup;
		}
		set {
			optionGroup = onlyOneSelected ? value.UniqueSelectOneAndOnlyOne() : value;
			sycnViewWithOptionGroup();
		}
	}

	protected virtual void sycnViewWithOptionGroup ()
	{
		if (OptionGroup.AllOptionNames.Count() != uiOptions.Count ()) {
			throw new Exception(
				string.Format("uiOptions count error. Expected:[{0}], actual:[{1}]", 
			              OptionGroup.AllOptionNames.Count(), uiOptions.Count ())
			);
		}
		foreach (int i in Enumerable.Range(0, uiOptions.Count())) {
			uiOptions.ElementAt(i).Option = OptionGroup.Options.ElementAt(i);
		}
	}

	[SerializeField]
	List<UIOption> uiOptions;
	void Awake() {
		uiOptions = GetComponentsInChildren<UIOption> ().ToList ();
		foreach (var ui in uiOptions) {
			ui.Interaction += onUIOption;
		}
	}

	void onUIOption(object sender, EventArgs e) {
		var ui = sender as UIOption;
		OptionGroup = onlyOneSelected ? OptionGroup.UniqueSelectOne(ui.Option.Name) :
			OptionGroup.SpecifySelectOne (ui.Option.Name, ui.Option.Selected);
		OnInteraction ();
	}
}

// manage Option.
public abstract class UIOption : MonoBehaviour {
	public abstract Option Option {get;set;}
	
	public event EventHandler Interaction;
	// ensure Option value already changed before calling this function
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}
