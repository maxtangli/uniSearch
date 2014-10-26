using UnityEngine;
using System.Collections;

public class UINullSorter : UISorter {
	#region implemented abstract members of UISorter
	SorterData sorterData;
	public override SorterData SorterData {
		get {
			return sorterData;
		}
		set {
			this.sorterData = value;
		}
	}
	#endregion
}
