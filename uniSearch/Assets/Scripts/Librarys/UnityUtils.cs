using UnityEngine;
using System.Collections;
using System;

public static class UnityUtils {
	// Instantiate a prefab which contains a T component, and init it.
	public static T InstantiatePrefab<T>(T prefab, Action<T> init = null) where T:MonoBehaviour {
		init = init ?? (x => {});

		GameObject go = GameObject.Instantiate (prefab.gameObject) as GameObject;
		T component = go.GetComponent<T> ();
		init (component);
		return component;
	}
}
