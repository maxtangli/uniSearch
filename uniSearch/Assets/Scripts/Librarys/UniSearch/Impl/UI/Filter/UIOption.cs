using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// manage Option.
public abstract class UIOption : MonoBehaviour {
	public abstract OptionData OptionData {get;set;}
	
	public event EventHandler Interaction;
	// ensure Option value already changed before calling this function
	protected void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}