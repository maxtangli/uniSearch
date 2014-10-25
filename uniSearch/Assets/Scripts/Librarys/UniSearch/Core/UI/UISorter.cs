using UnityEngine;
using System.Collections;
using System;
public abstract class UISorter : MonoBehaviour {
	public abstract SorterData SorterData {get;set;}
	public event EventHandler Interaction;
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}
