using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class SearchController : MonoBehaviour {

	public UIFilter uiFilter;

	void Awake() {
		initUISearch ();
		uiFilter.Interaction += onUIFilter;
	}

	void onUIFilter(object sender, System.EventArgs e) {
		OptionGroup g = uiFilter.OptionGroups.First ();
		Debug.Log (g);
	}

	[ContextMenu("initUISearch")]
	void initUISearch() {
		IEnumerable<OptionGroup> filterCondtions = new List<OptionGroup> () {
			new OptionGroup("rarity",new string[] {"N","R","SR"})
		};
		uiFilter.OptionGroups = filterCondtions;
	}
	
}
