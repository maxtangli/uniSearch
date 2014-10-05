using UnityEngine;
using System.Collections;
using System;

public static class UnityUtils {
	public static T InstantiatePrefab<T>(T prefab, Action<T> init = null) where T:MonoBehaviour {
		init = init ?? (x => {});

		GameObject go = GameObject.Instantiate (prefab.gameObject) as GameObject;
		T component = go.GetComponent<T> ();
		init (component);
		return component;
	}
}
