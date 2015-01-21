uniSearch(under developing)
=========

A practice aiming at a library to simplify Unity UI filter/sorter/pager workflow.

## TODO
- [x] A draft version of readme.
- [x] Improve readability. The summary page should be brief, and should be eaily understand by readers. 
- [ ] Add a tutorial video.

## User Story: a novice unity programmer's one day.

As a novice Unity Programmer in a poker game project, you are asked to made a poker galary scene like this: 

![Requirements](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image2.jpg)

In this weel designed project, you find these things helps: 
- PJ-reusable Card class
- PJ-reusable UICardImage prefab
- NGUI UIGrid class 
- Unity Editor Hotkeys

You get things below in **1 minute, with the cost of about 20 key/mouse operations and 0 lines of code**.

![1 Minute Result](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image1.jpg)

Here comes the question: **In a well designed project, what would be the cost to fullfil the requirement from this 1 minute result?**

UniSearch try to reach the goal in **4 minutes, with the cost of 2 customer-written classes and 1 pj-reusable prefab**.(Note that the steps will be more simpilified in future)

**1 UISearcher prefab**: A pj-reusable prefab with UISearcher MonoBehaviour script attached, which provides SearcherData under user interaction.

```C#
public abstract class UISearcher : MonoBehaviour
{
	public abstract SearcherData SearcherData {
		get;set;
	}
	public abstract int NumTotal {
		get;set;
	}
	public event EventHandler Interaction;
	public void OnInteraction() {
		if (Interaction != null) {
			Interaction(this,EventArgs.Empty);
		}
	}
}
```

**1 DataProvider Class**: A CardDataProvider class, wihch provide DataProviderResult for given SearcherData.

```C#
public class CardDataProvider : IDataProvider<Card> {
	IEnumerableDataProvider<Card> provider;
	public CardDataProvider() {
		var allCards = Enumerable.Range (1, 52).Select (
			x => new Card ( CardUtil.CodeToCardColor(x), CardUtil.CodeToPoint(x)) );
		IFilter<Card> filter = new PredicatesFilter<Card> (
			new Dictionary<string, IDictionary<string, Predicate<Card>>>() {
				{
					"COLOR" ,new Dictionary<string, Predicate<Card>> () {
						{"HEART", x => x.Color == CardColor.HEART},
						{"SPADE", x => x.Color == CardColor.SPADE},
						{"CLUB", x => x.Color == CardColor.CLUB},
						{"DIAMOND", x => x.Color == CardColor.DIAMOND},
					}
				},
				{
					"POINT" ,new Dictionary<string, Predicate<Card>> () {
						{"A", x => x.Point == 1},
						{"2-5", x => x.Point >= 2 && x.Point <= 5},
						{"6-10", x => x.Point >= 6 && x.Point <= 10},
						{"J-K", x => x.Point >= 11},
					}
				}
			} 
		);
		provider = new IEnumerableDataProvider<Card> (allCards, filter);
	}
	#region implemented abstract members of DataProvider
	public void fetch (SearcherData searcherCondition, Action<DataProviderResult<Card>> onDataFetched)
	{
		provider.fetch (searcherCondition, onDataFetched);
	}
	public SearcherData SearcherCandidate { get {return provider.SearcherCandidate;} }
	#endregion
} 
```

**1 Controller Class**: A controller class to handle interactions between UISearcher and CardDataProvider.

```C#
public class PokerGallaryController : MonoBehaviour {
	CardDataProvider provider = new CardDataProvider();
	public UISearcher uiSearcher;
	public UIGrid uiGrid;
	void Start() {
		uiSearcher = uiSearcher ?? GetComponentInChildren<UISearcher> ();
		uiSearcher.Interaction += onUISearch;
		uiSearcher.SearcherData = provider.SearcherCandidate;
		uiSearcher.OnInteraction ();
	}
	void onUISearch(object sender, EventArgs e) {
		provider.fetch (uiSearcher.SearcherData, onDataFetched);
	}
	void onDataFetched (DataProviderResult<Card> result)
	{
		uiSearcher.NumTotal = result.numFiltered;
		var prefabs = result.datas.Select (card => UICardImage.InstantiatePrefab (card).gameObject).ToList ();
		uiGrid.setContents(prefabs);
	}
}
```

Here's the 5-minutes result:

![5 Minutes Result](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image3.jpg)

Finishing today's task in 5 minutes, you reported your work and ask if any other task to do, but the main programmer replyed as below:

> "You should understand why you are assigned 8 hours for a 5-mintues-task. 
> In our company, the main responsibility for a novice engineer IS NOT to do low-technology-level tasks, which has been almost eliminated by frameworks, libraries and components made by our master engineers! 
> Your main responsibility IS learning, learning and learning! 
> Best wishes for your grow up and the day that you join our master engineers and do real coding!"

Feeling moving and encouraged, you go to the company library and pick up some books in topic of Object-Oriented Design.
Taking the book back to your working desk, sit down, you said to yourself:
"Now it's the real begining of my today's work!"

Point

- Components saves time.
- Time saved by components enable us to make more components and saves more time.
