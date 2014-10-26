using UnityEngine;
using System.Collections;
using System;

// Manage RangerData.
public abstract class UIRanger : MonoBehaviour {
	public abstract RangerData RangerData {
		get;set;
	}
	// typically, numTotal should be set BEFORE RangerData.
	public abstract int NumTotal {
		get;set;
	}
	// when filter/sorter changed, rangerData should be reset to default.
	public abstract RangerData DefaultRangerData {
		get;
	}
	public event EventHandler Interaction;
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}

