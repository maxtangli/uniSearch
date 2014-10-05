using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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