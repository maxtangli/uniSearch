using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Immutable pager class.
[System.Serializable]
public class PagerData {
	// data
	[SerializeField]int numRecord;
	[SerializeField]int numRecrodPerPage;
	[SerializeField]int nowPage;
	public PagerData(int numRecord, int numRecrodPerPage, int nowPage = 0) {
		if (!PagerDataUtil.ValidNowPage (numRecord, numRecrodPerPage, nowPage)) {
			throw new System.ArgumentException(string.Format("Invalid nowPage: [{0}]"));
		}
		this.numRecord = numRecord;
		this.numRecrodPerPage = numRecrodPerPage;
		this.nowPage = nowPage;
	}
	
	// direct property
	public int NumRecord {
		get {return numRecord;}
	}
	public int NumRecrodPerPage {
		get {return numRecrodPerPage;}
	}
	public int NowPage {
		get {return nowPage;}
	}

	// methods
	public PagerData ChangePage(int newNowPage) {
		return new PagerData(NumRecord, NumRecrodPerPage, newNowPage);
	}
	public PagerData ChangePageByIndex(int newNowPageFirstIndex) {
		int newNowPage = PagerDataUtil.IndexToPage (NumRecord, NumRecrodPerPage, newNowPageFirstIndex);
		return ChangePage (newNowPage);
	}

	// extended property
	// page.
	public int NumPage {get {return PagerDataUtil.GetNumPage (NumRecord, numRecrodPerPage);}}
	public int FirstPage { get {return 0;} }
	public int LastPage { get {return NumPage > 0 ? NumPage - 1 : 0;}}
	public int PrevPage { get {return NowPage != FirstPage ? NowPage - 1 : NowPage;} }
	public int NextPage { get {return NowPage != LastPage ? NowPage + 1 : NowPage;} }
	// index is zero based, commonly used for array index.
	public int FirstIndex {get {return 0;}}
	public int LastIndex {get {return NumRecord > 0 ? NumRecord - 1 : 0;}}
	public int NowPageFirstIndex {get {return NowPage * NumRecrodPerPage;}}
	public int NowPageLastIndex {get {return NowPage != LastPage ? NowPageFirstIndex + NumRecrodPerPage - 1 : LastIndex;}}
	public int NowPageNumRecord {get {return NowPageLastIndex - NowPageFirstIndex + 1;}}
	// number is one-based, commonly used for page number display.
	public int NowPageNumber {get {return NowPage + 1;}}
	public int NowPageFirstRecordNumber {get {return NowPageFirstIndex + 1;}}
	public int NowPageLastRecordNumber {get {return NowPageLastIndex + 1;}}	
}

public static class PagerDataUtil {
	public static int GetNumPage(int numRecord, int numRecrodPerPage) {
		return (int)System.Math.Ceiling(((double)numRecord) / numRecrodPerPage);
	}
	public static int IndexToPage(int numRecord, int numRecordPerPage, int index) {
		if (!ValidIndex (numRecord, index)) {
			throw new System.ArgumentException();
		}
		return index % numRecordPerPage;
	}
	public static bool ValidNowPage(int numRecord, int numRecrodPerPage, int nowPage) {
		int numPage = GetNumPage(numRecord, numRecrodPerPage);
		return (nowPage >= 0 && nowPage < numPage) || (numPage == 0 && nowPage == 0);
	}
	public static bool ValidIndex(int numRecord, int index) {
		return (index >= 0 && index < numRecord) || (numRecord == 0 && index == 0);
	}
}
