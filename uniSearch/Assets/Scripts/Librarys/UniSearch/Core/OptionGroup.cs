using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// Selectable option data.
// Immutable to avoid pass-reference-unintend-modify issues
// NOTE: convinient functions to be re-organize.
// NOTE: performance not tuned (Not pretend to do until readlly need).
[System.Serializable]
public class Option {
	[SerializeField]
	string name; // NOTE: unity unable to serialize "readonly"
	public string Name {
		get {
			return name;
		}
	}
	[SerializeField]
	bool selected;
	public bool Selected {
		get {
			return selected;
		}
	}
	public Option (string name, bool selected)
	{
		this.name = name;
		this.selected = selected;
	}
	
	public override string ToString ()
	{
		return string.Format ("{0}{1}", Selected ? "+" : "-", Name);
	}	

	public override bool Equals (object obj)
	{
		if (obj == null) {
			return false;
		}

		OptionGroup g = obj as OptionGroup;
		if (g == null) {
			return false;
		}

		return Equals (g);
	}

	public bool Equals(Option obj) {
		if (obj == null) {
			return false;
		}

		return Name == obj.Name && Selected == obj.Selected;
	}

	public override int GetHashCode ()
	{
		return Name.GetHashCode() | Selected.GetHashCode();
	}

	#region Option convenient methods
	public Option select() {
		return selected ? this : new Option(Name, true);
	}
	public Option unSelect() {
		return !selected ? this : new Option(Name, false);
	}
	public Option antiSelect() {
		return new Option(name, !Selected);
	}
	#endregion
}

// A group of selectable option data.
// Immutable to avoid pass-reference-unintend-modify issues
[System.Serializable]
public class OptionGroup {
	[SerializeField]string title;
	public string Title {
		get {
			return title;
		}
	}

	[SerializeField]List<Option> options;
	public IEnumerable<Option> Options {
		get {
			return options;
		}
	}
	
	public OptionGroup (string title, IEnumerable<Option> options)
	{
		System.Diagnostics.Debug.Assert (title != null);
		System.Diagnostics.Debug.Assert (options != null && options.Count () > 1);
		this.options = options.ToList();
		this.title = title;
	}
	public OptionGroup (string title, IEnumerable<string> optionNames)
		: this(title, optionNames.Select(x => new Option(x, true)))
	{
	}
	
	public override string ToString ()
	{
		string optionStrs = string.Join (",", Options.Select (x => x.ToString ()).ToArray());
		return string.Format ("[{0}:{1}]", Title, optionStrs);
	}	

	public bool SameCandidate(OptionGroup g) {
		return Title == g.Title && AllOptionNames.SequenceEqual (g.AllOptionNames);
	}

	#region OptionGroup convenient methods
	// NOTE: better to use Extensions.
	public IEnumerable<string> AllOptionNames {
		get {
			return Options.Select(x => x.Name);
		}
	}
	public IEnumerable<string> SelectedOptionNames {
		get {
			return Options.Where(x => x.Selected).Select(x => x.Name);
		}
	}
	public IEnumerable<string> NotSelectedOptionNames {
		get {
			return Options.Where(x => !x.Selected).Select(x => x.Name);
		}
	}
	
	public OptionGroup SelectMany(IEnumerable<string> names) {
		return ChangeMany(names, x => x.select());
	}
	public OptionGroup SelectOne(string name) {
		return SelectMany(new string[] {name});
	}
	
	public OptionGroup UnSelectMany(IEnumerable<string> names) {
		return ChangeMany(names, x => x.unSelect());
	}
	public OptionGroup UnSelectOne(string name) {
		return UnSelectMany(new string[] {name});
	}
	
	public OptionGroup SpecifySelectOne(string name, bool selected) {
		return selected ? SelectOne (name) : UnSelectOne (name);
	}

	public OptionGroup UniqueSelectMany(IEnumerable<string> names) {
		return ChangeMany(names, x => names.Contains(x.Name) ? x.select() : x.unSelect());
	}
	public OptionGroup UniqueSelectOne(string name) {
		return UniqueSelectMany(new string[] {name});
	}
	public OptionGroup UniqueSelectOneAndOnlyOne() {
		int selectedCount = SelectedOptionNames.Count();
		if (selectedCount == 1) {
			return this;
		} else if (selectedCount == 0){
			return UniqueSelectOne(AllOptionNames.First());
		} else {
			return UniqueSelectOne(SelectedOptionNames.First());
		}
	}

	public OptionGroup AntiSelectMany(IEnumerable<string> names) {
		return ChangeMany(names, x => names.Contains(x.Name) ? x.antiSelect() : x);
	}
	public OptionGroup AntiSelectOne(string name) {
		return AntiSelectMany(new string[] {name});
	}

	// change option's names while keep select status
	public OptionGroup MapOptionNames(IEnumerable<string> newOptionNames) {
		System.Diagnostics.Debug.Assert (Options.Count () == newOptionNames.Count ());
		var selectStatus = Options.Select (x => x.Selected);
		var newOptions = newOptionNames.Zip(selectStatus, 
		                                       (newOptionName, selected) => new Option(newOptionName, selected));
		return new OptionGroup (Title, newOptions);
	}

	OptionGroup ChangeMany(IEnumerable<string> names, Func<Option,Option> gen) {
		System.Diagnostics.Debug.Assert(validNames(names));
		var resultOptions = Options.Select(x => gen(x));
		var result = new OptionGroup(Title, resultOptions);
		return result;
	}
	bool validNames (IEnumerable<string> names)
	{
		return names.All(x => AllOptionNames.Contains(x));
	}
	#endregion
}

public static class OptionGroupExtensions {
	public static bool ValidConditions(this IEnumerable<OptionGroup> candidates, IEnumerable<OptionGroup> conditions) {
		return conditions.All (condition => 
		                       candidates.Any(candidate => candidate.SameCandidate (condition)));
	}
}