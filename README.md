uniSearch(developing)
=========

A practice aiming at a library to simplify Unity UI filter/sorter/pager workflow.

## User Story: a novice unity programmer's happy workday.

Suppose you are an novice Unity Programmer in a poker game project, and you are asked to made a scene of poker galary.

Here's the requirements: 

1. There are totally 52 kinds of poker cards.
2. Show 5 cards per pages.
3. User can choose which the color and point of poker cards to shown.

Since the project is well organized and designed, you soon find two classes made by Main Programmer can be reused here:

1. Card class, which means card data.
2. UICardImage prefab, the view of a card, which can be customized by UICardImage.set(Card card)

Thus, you move into your scene (with UIRoot and other things prepared) begin your work by inspector:

1. Create a GameObject, Add a UIGrid. (4 key stroke: Ctrl+N, Ctrl+A, g, Enter)
2. Drag 5 UICardImage Prefab into UIGrid. (5 mouse drag)
3. In UIGrid inspector, adjust Cell Width and click Execute. (3 key stroke for digit "140", 1 mouse click) 

You are happy to made a raw view without search support in 1 minute, as picture below shows, with just 13 key/mouse operations and 0 lines of code.

![Image1](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image1.jpg)

Here comes the question: In such a well designed project, what would be the time cost to made things like image2 from image1?

![Image2](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image2.jpg)

Here's your step with UniSearch:
(NOTE: steps will be more simplified in future)

1. Write a CardDataProvider class to support searching. 

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

2. Made a UISearcher by inspector.
- Create a GameObject, add a UIFSRSearcher.
- Drag a PJFilter Prefab into it, set properly amount of children by copy&paste.
- Drag a PJPager Prefab into it.

3. Write a PokerGallaryController class to handle interactions of UISearcher by CardDataProvider:

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

4. Create a GameObject, Add PokerGallaryController.
5. Run.

![Image3](https://github.com/maxtangli/uniSearch/blob/master/screenshot/image3.jpg)

You finished these things in 4 minutes. Add by the prior 1 minute, you spent totally 5 minutes to get today's task done.

You reported your task and ask if any other task to do, but the main programmer replyed as below:

"You should understand why you are assigned 8 hours for a 5-mintues-task. 
In our company, the main responsibility for a novice engineer IS NOT to do low-technology-level tasks, which has been almost eliminated by frameworks, libraries and components made by our master engineers! 
Your main responsibility IS learning, learning and learning! 
Best wishes for your grow up and the day that you join our master engineers and do real coding!"

Feeling moving and encouraged, you go to the book store and pick up some books in topic of Object-Oriented Design.
Taking the book back to your working desk, sit down, you said to yourself:
"Now it's the start for my today's work!"

Point

- Components saves time.
- Time saved by components enable us to make more components.

## Overview

Data Querying UI(filter/sorter/pager) is common feature in games. 
In Unity it's easy to made such things, but since no standard solution exists:
- You should write code to handle trivial things: interaction, get data for searching condition in UI, update searching condition, etc.
- You may failed to consider some cases: maintainability, flexibility, error handling, etc.
- Hard to reuse in and between projects.
- Not enough convenience: In a project, Querying UI is tend to provide little search conditions (since complex ones cost labours), which do not provide enough convenience for users.  

Addressing at this, UniSearch provide a standard solution such that:
- Least lines of code is need.
- Don't make you think, since all cases handled properly.
- Reusable.
- Complex searching UI is OK since its labour cost is nearly the same as simple ones. 

In UniSearch, the process is simplified by three parts:

1. Model

In most cases, the mainly code you need to write is a IDataProvider that provide datas for searching conditions,
which is typically implemented by few lines:

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

2. View
Each project will customize a UISearch Prefab for all searching usecases.
TODO

3. Controller
In beta version you still need to write a simple controller between IDataProvider and UISearcher.

In late version this should be eliminated by a standard search controller.

## Q&A

### Q: このPJをやった理由？
仕事でコーティングする時、「一番よいデザイン・手順」より、「PJのデザインと合う、一番慣れる方法で早めに完成」が基本ですが、「難しくないが毎回同じようなものを作っている」「じっくり考えたらもっとよりデザインがあるはず」の感覚がよく出てきます。設計力を上げるため、個人PJをしようと考え、業務でよく作った「Filter/Sorter/Pagerなど検索UIのライブラリー化」をテーマとしました。

### Q: 困難は？
最初の困難は「全然初められない！」（笑）。使いやすいの形は想像出来なくて、最初は何時間も考えても何も出てこないです。それに対し、「とりあいず何か作ってみよう」と思って、デザインをなしにコーティングを始めました。結果：動けるものが出ってきましたが、明らかにライブラリーとして使えないものです。でもそのおかげて、使いやすいの形のイメージがだんだん出てきました。（最初のコードはsvn_xxxをcheckoutすれば見られます。）

途中出てきた困難は設計力不足です。コーティングすればするほど、「何故何時間もかけるのに、これくらいのコードしか出ってこない？」と気づいて、考えて見ると、「ロジックが複雑すぎて毎回ちゃんっと分析しなければならない」=>「どうすればDon't make me thinkingになれる？」、「複数のクラスを組み合わせるのは難しい」=>「どう設計すれば必ず使いやすい？」。対策としてオブジェクト指向デザインのレベルアップを決めました。[Agile Software Developmentの読書と練習](https://github.com/maxtangli/Personal/tree/master/2014.08_CSharp_EmployeePayment)をした後、デザインが前より良くなり、コーティングのスピードの改善が見られます。

### Q: 成長点は？
Unity・C#・設計力などの専門知識は無論、勉強の方法論はもっと重要だと思います。

今回の場合、以下の流れが幾つあります：

1. （「何の困難をあったか」より）「設計が複雑、スムーズに進めていない」＝＞「オブジェクト指向設計の手順が正しくない？」＝＞「オブジェクト指向設計の本を読む」＝＞「設計力のアップ」
2. 「簡単なことなのにC#でするのはこんなに難しい？」＝＞「ここで知らなかったC#のテクニックがあるかも」＝＞「C#の資料を読む」＝＞「なるほど、C#はparams keyword/ sturct / value object patternがある！」

まとめてみると、練習の中で、「（違和感より）問題点の気づき」＝＞「原因の分析より、足りない能力・知識の見つけ出し」＝＞「不足対しの勉強案の計画と実行」＝＞「効果の観測」のサイクルより、レベルアップのことができます。

（注：おおげさ・かたいすぎるかな？）

### Q: 検索UIは簡単な機能と思って、ライブラリー化の価値が本当にあるか？
（本来は練習の目的ですが）PJで、検索UIのためわざわざライブラリーを使うのは大袈裟と思いますが、もしNGUIのようなUIライブラリーの一部として提供すれば、使う人もあるでしょう
。

### Q: 本格的にライブラリーを作る場合、どんな流れが良いと思う？
個人好み用ならどうでもいいと思うか、本格的な場合は事前計画をしっかりしないと、「必要ない・使いにくいものが出ってしまう」「膨大な時間の浪費」などのリスクが出やすいと思う。

本格的にライブラリーを作る場合、以下の流れが良いと思う：

1. 作る価値があるかの判断。現状・問題点の分析は無論、ユーザーいわゆるプログラマーたちに聞くのも必要だと思う。
2. 現状の調査。既に存在する関連ライブラリーを調べ、それぞれの持つ特性・利点・欠点をまとめて、今回のライブラリーの以来より良い特性・利点を決める。
3. demoの作成。最初は一番重要な特性だけを、質より開発速度重視の形で作り、自分またはユーザーにフィードバックをもらう。
4. betaの作成。今回は重要な特性をピックアップし、質重視の形で作り、出来たら公開してまたユーザーにフィードバックをもらう。
5. 改善。良いと思うユーザーが多いだったら、修正、改善、特性追加を進む。

### Q: このページに対する感想？
コードより、最初のuser storyの書き方はもっと良い。
いつか広く使われているライブラリーのuser stroyを書ければいいな～