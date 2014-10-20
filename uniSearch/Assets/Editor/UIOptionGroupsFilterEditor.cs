using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(UIOptionGroupsFilterEditor))]
public class UIOptionGroupsFilterEditor : Editor  {

	bool showOptionGroups = true;
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		UIOptionGroupsFilter g = (UIOptionGroupsFilter)target;
		EditorGUILayout.BeginVertical ();
		showOptionGroups = EditorGUILayout.Foldout(showOptionGroups, "OptionGroups");
		if (showOptionGroups) {
			foreach(var optionGroup in g.FilterData) {
				EditorGUILayout.LabelField (
					optionGroup.Title + ":" + string.Concat(optionGroup.ToArray()));
			}
		}
		EditorGUILayout.EndVertical ();
	}
}
