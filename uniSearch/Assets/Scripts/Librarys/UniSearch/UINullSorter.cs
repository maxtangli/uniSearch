using UnityEngine;
using System.Collections;

public class UINullSorter : UISorter {
	#region implemented abstract members of UISorter
	public override SorterData SorterData {
		get {
			return new SorterData();
		}
		set {

		}
	}
	#endregion
	
}
