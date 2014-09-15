using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// A list of selectable Options.

// Manage a list of OptionGroup as filter condition.
public abstract class UIFilter : MonoBehaviour {
	public abstract IEnumerable<OptionGroup> OptionGroups {get;set;}
	public event EventHandler Interaction;
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}

// Manage OptionGroup.

// A Null UIOptionGroup which provide debug operations in inspector. 
