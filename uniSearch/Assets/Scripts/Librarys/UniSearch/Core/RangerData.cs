using UnityEngine;
using System.Collections;

// Immutable int-range class.
[System.Serializable]
public class RangerData {
	[SerializeField]int index;
	[SerializeField]int count;
	public int Index {
		get {
			return this.index;
		}
	}
	public int Count {
		get {
			return this.count;
		}
	}
	public int LastIndex {
		get {
			return Index + Count - 1;
		}
	}

	public RangerData (int index = 0, int count = int.MaxValue)
	{
		bool valid = index >= 0 && count >= 1;
		if (!valid) {
			throw new System.ArgumentException();
		}
		this.index = index;
		this.count = count;
	}
	public override string ToString ()
	{
		return string.Format ("[Range:{0}-{1}]", Index, LastIndex);
	}
}
