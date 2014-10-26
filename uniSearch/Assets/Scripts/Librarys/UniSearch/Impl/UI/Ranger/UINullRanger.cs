using UnityEngine;
using System.Collections;
using System;

// Manage RangerData.

public class UINullRanger : UIRanger {
	#region implemented abstract members of UIRanger

	RangerData rangerData;
	public override RangerData RangerData {
		get {
			return rangerData;
		}
		set {
			this.rangerData = value;
		}
	}

	int numTotal;
	public override int NumTotal {
		get {
			return numTotal;
		}
		set {
			this.numTotal = value;
		}
	}

	public override RangerData DefaultRangerData {
		get {
			return new RangerData();
		}
	}

	#endregion

}