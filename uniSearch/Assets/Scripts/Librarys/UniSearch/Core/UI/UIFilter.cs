using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Manage a list of OptionGroup as filter condition.
public abstract class UIFilter : MonoBehaviour {
	public abstract FilterData FilterData {get;set;}
	public event EventHandler Interaction;
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}
