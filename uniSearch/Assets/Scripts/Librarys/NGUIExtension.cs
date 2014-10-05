using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// TODO add LOCAL_TRANSFORM_PATTERN to public interfaces
public static class NGUIExtension {
	public static void setContents(this UIScrollView view, UIGrid grid, IEnumerable<GameObject> newContents) {
		if (view == null || newContents == null) {
			throw new System.ArgumentNullException();
		}

		view.ResetPosition();
		if (grid != null) {
			grid.setContents (newContents);
		} else {
			setContents(view.gameObject, newContents);
		}				
		view.ResetPosition();
	}

	public static void setContents(this UIGrid parent, IEnumerable<GameObject> newContents) {
		if (parent == null || newContents == null) {
			throw new System.ArgumentNullException();
		}

		setContents (parent.gameObject, newContents);
		parent.Reposition ();
	}

	static void setContents(GameObject parent, IEnumerable<GameObject> newContents) {
		if (parent == null || newContents == null) {
			throw new System.ArgumentNullException();
		}

		// clear oldContents
		var pt = parent.transform;
		for (int i = pt.childCount-1; i >=0; --i) {
			GameObject oldContent = pt.GetChild(i).gameObject;
			if (newContents.Contains(oldContent)) {
				throw new System.NotImplementedException(
					"As an agile beliver, I won't implement this uncommon issue until it becomes really need.");
			}
			oldContent.transform.parent = null;
			GameObject.Destroy(oldContent);
		}

		// add newContents to parent
		foreach(GameObject content in newContents) {
			AddChild(parent, content);
		}
	}

	enum LOCAL_TRANSFORM_PATTERN {Keep = 0, Free = 1, Identity = 2}
	static void AddChild (GameObject parent, GameObject child, 
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