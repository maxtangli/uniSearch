using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// A Null UIOptionGroup which provide debug operations in inspector. 
public class UINullOptionGroup : UIOptionGroup {
	#region implemented abstract members of UIOptionGroup

	[SerializeField]
	OptionGroup optionGroup;
	public override OptionGroup OptionGroup {
		get {
			return optionGroup;
		}
		set {
			optionGroup = value;
		}
	}

	#endregion

	[ContextMenu("selectAll")]
	public void selectAll() {
		OptionGroup.selectAll ();
		OnInteraction ();
	}
	[ContextMenu("antiSelect1st")]
	public void antiSelect1st() {
		OptionGroup.antiSelectOne (0);
		OnInteraction ();
	}
	[ContextMenu("antiSelect2nd")]
	public void antiSelect2nd() {
		OptionGroup.antiSelectOne (1);
		OnInteraction ();
	}
	[ContextMenu("antiSelect3rd")]
	public void antiSelect3rd() {
		OptionGroup.antiSelectOne (2);
		OnInteraction ();
	}	
}