using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Implement UIFilter by a mapping between List<OptionGroup> and List<UIOptionGroup>.
// FilterData.set : 

public class UINullFilter : UIFilter {
	#region implemented abstract members of UIFilter

	FilterData filterData;
	public override FilterData FilterData {
		get {
			return filterData;
		}
		set {
			filterData = value;
		}
	}

	#endregion

}