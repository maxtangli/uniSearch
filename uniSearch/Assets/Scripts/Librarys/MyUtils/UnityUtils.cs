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

	public enum LOCAL_TRANSFORM_PATTERN {Keep = 0, Free = 1, Identity = 2}
	public static void AddChild (GameObject parent, GameObject child, 
	                      LOCAL_TRANSFORM_PATTERN pattern = LOCAL_TRANSFORM_PATTERN.Keep ) {
		if (parent == null || child == null) {
			throw new System.ArgumentNullException();
		}
		
		Transform t = child.transform;
		if (pattern == LOCAL_TRANSFORM_PATTERN.Free) {
			t.parent = parent.transform;
		} else if (pattern == LOCAL_TRANSFORM_PATTERN.Keep) {
			var oldPosition = t.localPosition;
			var oldRotation = t.localRotation;
			var oldScale = t.localScale;
			t.parent = parent.transform;
			t.localPosition = oldPosition;
			t.localRotation = oldRotation;
			t.localScale = oldScale;
		} else if (pattern == LOCAL_TRANSFORM_PATTERN.Identity) {
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		} else {
			throw new System.ArgumentException();
		}
	}
}

/*
public static class IEnumerableExtensions {
	// By default, MonoDevelop's configuration not support IEnumerable.Zip which comes from .net 4.0.
	// TODO configure issues.
	// http://stackoverflow.com/questions/16927845/cs1061-cs0117-system-linq-enumerable-does-not-contain-a-definition-for-zip
	public static IEnumerable<T> Zip<A, B, T>(
		this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
	{
		if (seqA == null) throw new ArgumentNullException("seqA");
		if (seqB == null) throw new ArgumentNullException("seqB");
		
		using (var iteratorA = seqA.GetEnumerator())
			using (var iteratorB = seqB.GetEnumerator())
		{
			while (iteratorA.MoveNext() && iteratorB.MoveNext())
			{
				yield return func(iteratorA.Current, iteratorB.Current);
			}
		}
	}
}*/