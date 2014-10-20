using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Implement UIOptionGroup by simple get/set. Typically used as NullPattern.
public class UIStubOptionGroup : UIOptionGroup {
	#region implemented abstract members of UIOptionGroup

	OptionGroupData optionGroupData;
	public override OptionGroupData OptionGroupData {
		get {
			return optionGroupData;
		}
		set {
			optionGroupData = value;
		}
	}

	#endregion

}