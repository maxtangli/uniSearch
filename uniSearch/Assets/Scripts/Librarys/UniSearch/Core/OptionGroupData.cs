using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Immutable data of a group of options.
public class OptionGroupData : IEnumerable<OptionData> {
	string title;
	public string Title {
		get {
			return title;
		}
	}
	
	IList<OptionData> optionDatas;
	
	public OptionGroupData(string title, IEnumerable<OptionData> optionDatas) {
		this.title = title;
		this.optionDatas = optionDatas.ToList();
	}
	
	public override string ToString ()
	{
		string optionStrs = string.Join (",", optionDatas.Select (x => x.ToString ()).ToArray());
		return string.Format ("[{0}:{1}]", Title, optionStrs);
	}
	
	// two OptionGroupData are same candidate, if
	// 1. same Title
	// 2. same Count of Options
	// 3. same order and Name of each Options
	public bool SameCandidate(OptionGroupData g) {
		return Title == g.Title && optionDatas.Select(x => x.Name).SequenceEqual(g.optionDatas.Select(x => x.Name));
	}

	#region Convenient Utils
	public IEnumerable<OptionData> OptionNamesAt(params int[] indexs) {
		var result = optionDatas.Where(x => indexs.Contains(optionDatas.IndexOf(x)));
		if (result.Count() != indexs.Count()) {
			throw new System.ArgumentException("Invalid indexs.");
		}
		return result;
	}
	public OptionGroupData SpecificCheck(bool newIsChecked, params string[] targetNames ) {
		return SetIsChecked (targetNames, (index, option) => newIsChecked);
	}
	public OptionGroupData Check(params string[] targetNames) {
		return SetIsChecked (targetNames, (index, option) => true);
	}
	public OptionGroupData UnCheck(params string[] targetNames) {
		return SetIsChecked (targetNames, (index, option) => false);
	}
	public OptionGroupData AntiCheck(params string[] targetNames) {
		return SetIsChecked (targetNames, (index, option) => !option.IsChecked);
	}
	public OptionGroupData UniqueCheck(params string[] targetNames) {
		return SetIsChecked(targetNames, (index, option) => true, (index, option) => false);
	}
	// change targets' isSelected by targetCallback, nonTargets' isSelected by nonTargetCallback, if given.
	OptionGroupData SetIsChecked(IEnumerable<string> targetNames, Func<int,OptionData,bool> targetCallback, Func<int,OptionData,bool> nonTargetCallback = null) {
		bool valid = targetNames.All(x => optionDatas.Select(y => y.Name).Contains(x));
		if (!valid) {
			throw new System.ArgumentException();
		}
		
		if (targetCallback == null) {
			targetCallback = (index, optionData) => optionData.IsChecked;
		}
		if (nonTargetCallback == null) {
			nonTargetCallback = (index, optionData) => optionData.IsChecked;
		}
		
		var newOptionDatas = new List<OptionData>();
		for (int i = 0; i < optionDatas.Count; ++i) {
			var oldOptionData = optionDatas[i];
			
			bool newIsSelected = targetNames.Contains(oldOptionData.Name) ? targetCallback(i, oldOptionData) : nonTargetCallback(i, oldOptionData);
			var newOptionData = new OptionData(oldOptionData.Name, newIsSelected);
			
			newOptionDatas.Add(newOptionData);
		}

		return new OptionGroupData(title, newOptionDatas);
	}
	#endregion
	
	#region IEnumerable implementation
	
	public IEnumerator<OptionData> GetEnumerator ()
	{
		return optionDatas.GetEnumerator();
	}
	
	#endregion
	
	#region IEnumerable implementation
	
	IEnumerator IEnumerable.GetEnumerator ()
	{
		return optionDatas.GetEnumerator();
	}
	
	#endregion
}