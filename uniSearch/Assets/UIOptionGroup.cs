using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// A list of selectable Options.

// Manage a list of OptionGroup as filter condition.

// Manage OptionGroup.
public abstract class UIOptionGroup : MonoBehaviour {
	public abstract OptionGroup OptionGroup {get;set;}
	
	public event EventHandler Interaction;
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}

// A Null UIOptionGroup which provide debug operations in inspector. 
