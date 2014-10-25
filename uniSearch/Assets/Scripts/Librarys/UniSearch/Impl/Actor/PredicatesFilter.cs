using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// NOTE: a suceess of KISS!
public class PredicatesFilter<T> : IFilter<T> {
	IDictionary<string, IDictionary<string, Predicate<T>>> predicatesDict;
	// TODO Systen.Tuple is more convenient. But monoDevelop's support for .net 4.0? 
	public PredicatesFilter (IDictionary<string, IDictionary<string, Predicate<T>>> predicatesDict)
	{
		this.predicatesDict = predicatesDict;
	}

	#region IFilters implementation
	public bool match (T obj, FilterData conditions)
	{
		// TODO robust?
		return conditions.All(optionGroup => optionGroup.Where(option => option.IsChecked).Any(
			option => predicatesDict[optionGroup.Title][option.Name](obj)));
		// Q: Is it right IN PERFORMANCE to write this complex LINQ?
		// A: Since filterData.Count typically very little (< 20), no problem.
		// Q: How about general case?
		// TODO
	}
	public FilterData Candidates {
		get {
			return new FilterData(
				predicatesDict.Select(p => new OptionGroupData(
					p.Key, OptionData.CreateMany(p.Value.Select(p2 => p2.Key).ToArray())))
			);
		}
	}
	#endregion
}