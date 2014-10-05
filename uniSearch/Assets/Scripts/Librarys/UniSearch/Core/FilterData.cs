using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Immutable data for filter.
public class FilterData : IEnumerable<OptionGroup> {

	IEnumerable<OptionGroup> optionGroups;
	public FilterData (IEnumerable<OptionGroup> optionGroups)
	{
		this.optionGroups = optionGroups.Select(x => x);
	}

	#region IEnumerable implementation

	public IEnumerator<OptionGroup> GetEnumerator ()
	{
		return optionGroups.GetEnumerator ();
	}

	#endregion

	#region IEnumerable implementation

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return optionGroups.GetEnumerator ();
	}

	#endregion

	public override string ToString ()
	{
		return string.Format ("[{0}]", string.Join( ",", optionGroups.Select(x => x.ToString()).ToArray() ) );
	}
}