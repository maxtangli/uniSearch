using UnityEngine;
using System.Collections;
using System;

// Manager PagerData.
public abstract class UIPager : UIRanger {
	#region implemented abstract members of UIRanger
	public override RangerData RangerData {
		get {
			return new RangerData(PagerData.NowPageFirstIndex, PagerData.NumRecrodPerPage);
		}
		set {
			// keep numTotal, change nowPage
			PagerData = PagerData.ChangePageByIndex(value.Index);
		}
	}
	public override RangerData DefaultRangerData {
		get {
			return new RangerData(0, PagerData.NumRecrodPerPage);
		}
	}
	public override int numTotal {
		get {
			return PagerData.NumRecord;
		}
		set {
			// keep nowPage, change numTotal
			PagerData = new PagerData(value, PagerData.NumRecrodPerPage, PagerData.NowPage);
		}
	}
	#endregion

	public abstract PagerData PagerData {
		get;set;
	}
}
