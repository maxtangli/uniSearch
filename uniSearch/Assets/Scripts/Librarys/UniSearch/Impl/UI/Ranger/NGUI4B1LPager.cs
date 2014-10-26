using UnityEngine;
using System.Collections;
// Implement UIPager with 4 UIBUtton + 1 UILabel.
public class NGUI4B1LPager : UIPager {
	[SerializeField]
	PagerData pagerData;
		
	#region implemented abstract members of UIPager

	public override PagerData PagerData {
		get {
			return pagerData;
		}
		set {
			pagerData = value;
			syncView();
		}
	}
	#endregion

	public UIButton toFirst;
	public UIButton toPrev;
	public UIButton toNext;
	public UIButton toLast;
	public UILabel info;
	void syncView() {
		toFirst.isEnabled = PagerData.NowPage != PagerData.FirstPage;
		toPrev.isEnabled = PagerData.NowPage != PagerData.FirstPage;
		toNext.isEnabled = PagerData.NowPage != PagerData.LastPage;
		toLast.isEnabled = PagerData.NowPage != PagerData.LastPage;
		info.text = string.Format ("Page {0} / {1}. Record Now {2} - {3}, Total {4}.",
		                           PagerData.NowPageNumber, 
		                           PagerData.NumPage, 
		                           PagerData.NowPageFirstRecordNumber,
		                           PagerData.NowPageLastRecordNumber,
		                           PagerData.NumRecord);
	}

	void Awake() {
		foreach (var btn in new UIButton[] {toFirst, toPrev, toNext, toLast}) {
			EventDelegate.Add(btn.onClick, OnButtonClick);
		}
	}

	void OnButtonClick() {
		var btn = UIButton.current;
		if (btn == toFirst) {
			PagerData = PagerData.ChangePage(PagerData.FirstPage);
		} else if (btn == toPrev) {
			PagerData = PagerData.ChangePage(PagerData.PrevPage);
		} else if (btn == toNext) {
			PagerData = PagerData.ChangePage(PagerData.NextPage);
		} else if (btn == toLast) {
			PagerData = PagerData.ChangePage(PagerData.LastPage);
		} else throw new System.InvalidOperationException("impossible");

		OnInteraction ();
	}

	void OnValidate() {
		syncView ();
	}
}
