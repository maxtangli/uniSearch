using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Immutable data for filter.
public class FilterData : IEnumerable<OptionGroupData> {

	IEnumerable<OptionGroupData> optionGroupDatas;
	public FilterData (IEnumerable<OptionGroupData> optionGroupDatas)
	{
		bool valid = optionGroupDatas.Select (x => x.Title).Distinct ().Count () == optionGroupDatas.Count ();
		if (!valid) {
			throw new System.ArgumentException();
		}

		this.optionGroupDatas = optionGroupDatas.ToList();
	}

	// An other FilterData is a condition of this whenn all its OptionGroupData exist in this. 
	public bool IsCondition(FilterData other) {
		return other.All (x => this.Select(y => y.Title).Contains (x.Title));
	}
	
	public FilterData SpecificCheck(OptionGroupData newOptionGroupData) {
		string targetTitle = newOptionGroupData.Title;

		bool valid = optionGroupDatas.Any (x => x.Title == targetTitle);
		if (!valid) {
			throw new System.ArgumentException();
		}

		var newDatas = optionGroupDatas.Select(
			x => x.Title == targetTitle ? newOptionGroupData : x);

		return new FilterData (newDatas);
	}

	public override string ToString ()
	{
		return string.Format ("[{0}]", string.Join( ",", optionGroupDatas.Select(x => x.ToString()).ToArray() ) );
	}

	#region IEnumerable implementation

	public IEnumerator<OptionGroupData> GetEnumerator ()
	{
		return optionGroupDatas.GetEnumerator ();
	}

	#endregion

	#region IEnumerable implementation

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return optionGroupDatas.GetEnumerator ();
	}

	#endregion	
}