using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// implement UIOptionGroup by dynamic generation of specified prefab.
[RequireComponent(typeof(UIGrid))]
public class UIDynamicOptionGrid : UIOptionGroup {
	#region implemented abstract members of UIOptionGroup

	[SerializeField]
	OptionGroupData optionGroupData;
	public override OptionGroupData OptionGroupData {
		get {
			return optionGroupData;
		}
		set {
			optionGroupData = value;
			sycnViewWithOptionGroup();
		}
	}

	#endregion

	public UIOption prefab;
	IList<UIOption> initedPrefabs = new List<UIOption>();
	protected virtual void sycnViewWithOptionGroup ()
	{
		if (initedPrefabs.Count () > 0) {
			foreach(var p in initedPrefabs) {
				Destroy(p);
			}
			initedPrefabs.Clear();
		}

		foreach (var optionData in OptionGroupData) {
			var p = UnityUtils.InstantiatePrefab(prefab, x => x.OptionData = optionData);
			UnityUtils.AddChild(gameObject, p.gameObject, UnityUtils.LOCAL_TRANSFORM_PATTERN.Keep);
			initedPrefabs.Add(p);
		}
	}
}