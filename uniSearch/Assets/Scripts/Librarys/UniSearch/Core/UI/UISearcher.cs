using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Manage SearcherData.
public abstract class UISearcher : MonoBehaviour
{
	public abstract SearcherData SearcherData {
		get;set;
	}
	public abstract int NumTotal {
		get;set;
	}
	public event EventHandler Interaction;
	public void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}