using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// TODO propertyDrawer
// A list of selectable Options.
[System.Serializable]
public class OptionGroup {
	
	[System.Serializable]
	public class Option {
		[SerializeField]
		string optionName;
		public string OptionName {
			get {
				return optionName;
			}
			protected set {
				optionName = value;
			}
		}

		[SerializeField]
		bool selected;
		public bool Selected {
			get {
				return selected;
			}
			set {
				selected = value;
			}
		}

		public Option (string optionName, bool selected)
		{
			this.OptionName = optionName;
			this.Selected = selected;
		}

		public override string ToString ()
		{
			return string.Format ("{0}{1}", Selected ? "+" : "-", OptionName);
		}
	}

	[SerializeField]
	string title; // exist to adapt unity inspector
	public string Title { 
		get {
			return title;
		} 
		protected set {
			this.title = value;
		}
	}

	[SerializeField]
	List<Option> options = new List<Option>(); // exist to adapt unity inspector
	public IEnumerable<Option> Options {
		get {
			return options;
		}
		protected set {
			options = value.ToList();
		}
	}

	public OptionGroup (string title, IEnumerable<Option> options)
	{
		this.Title = title;
		this.Options = options;
	}
	public OptionGroup (string title, IEnumerable<String> optionNames) 
		: this(title, optionNames.Select(x => new Option(x,true)))
	{
	}

	public override string ToString ()
	{
		string optionStrs = string.Join (",", Options.Select (x => x.ToString ()).ToArray());
		return string.Format ("{0}:{1}", Title, optionStrs);
	}

	// --- convenient methods
	public void selectAll() {
		foreach (var o in Options)
			o.Selected = true;
	}
	public void unSelectAll() {
		foreach (var o in Options)
			o.Selected = false;
	}
	public void selectOne(int index) {
		Options.ElementAt (index).Selected = true;
	}
	public void unSelectOne(int index) {
		Options.ElementAt (index).Selected = false;
	}
	public void antiSelectOne(int index) {
		var o = Options.ElementAt (index);
		o.Selected = !o.Selected;
	}
	public void selectOne(string optionName) {
		Options.Where (x => x.OptionName == optionName).Single ().Selected = true;
	}
	public void unSelectOne(string optionName) {
		Options.Where (x => x.OptionName == optionName).Single ().Selected = false;
	}
	public void antiSelectOne(string optionName) {
		var o = Options.Where (x => x.OptionName == optionName).Single ();
		o.Selected = !o.Selected;
	}
}
