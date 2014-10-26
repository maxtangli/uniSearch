using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Immutable data of an option.
[System.Serializable]
public class OptionData {
	string name;
	public string Name {
		get {
			return name;
		}
	}
	
	bool isChecked;
	public bool IsChecked {
		get {
			return isChecked;
		}
	}
	
	public OptionData (string name, bool isChecked)
	{
		this.name = name;
		this.isChecked = isChecked;
	}
	
	public static IEnumerable<OptionData> CreateMany(params string[] names) {
		// Avoid Linq sideEffects.
		return names.Select(x => new OptionData(x, true)).ToList();
	}

	public OptionData SpecificCheck(bool newIsChecked) {
		return new OptionData (Name, newIsChecked);
	}
	public OptionData Check() {
		return SpecificCheck (true);
	}
	public OptionData UnCheck() {
		return SpecificCheck (false);
	}
	public OptionData AntiCheck() {
		return SpecificCheck (!IsChecked);
	}

	// NOTE: seems no meaning to compare with two OptionData
	/*
	public override string ToString ()
	{
		return string.Format ("{0}{1}", isChecked ? "+" : "-", Name);
	}	
	

	public override bool Equals (object obj)
	{
		if (obj == null) {
			return false;
		}
		
		OptionData o = obj as OptionData;
		if (o == null) {
			return false;
		}
		
		return Equals (o);
	}
	
	public bool Equals(OptionData obj) {
		if (obj == null) {
			return false;
		}
		
		return Name == obj.Name && IsChecked == obj.IsChecked;
	}
	
	public override int GetHashCode ()
	{
		return Name.GetHashCode() | IsChecked.GetHashCode();
	}*/
}
