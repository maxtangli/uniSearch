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
			foreach(var optionGroup in g.OptionGroups) {
				EditorGUILayout.LabelField (
					optionGroup.Title + ":" + string.Concat(optionGroup.Options.ToArray()));
			}
		}
		EditorGUILayout.EndVertical ();
	}
}
