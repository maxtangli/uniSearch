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