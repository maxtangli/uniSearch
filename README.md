uniSearch(developing)
=========

A practice aiming at a library to simplify Unity UI filter/sorter/pager workflow.

## TODO
- [x] A draft version of readme.
- [ ] Improve readability. The summary page should be brief, and should be eaily understand by readers. 
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

UniSearch try to reach the goal in **4 minutes, with the cost of 2 customer-written classes and 1 pj-reusable prefab**.

1 UISearcher prefab: A pj-reusable prefab with UISearcher MonoBehaviour script attached, which provides SearcherData under user interaction.

1 DataProvider Class: A CardDataProvider class, wihch provide DataProviderResult for given SearcherData.

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

1 Controller Class: A controller class to handle interactions between UISearcher and CardDataProvider.

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

## Q&A

### Q: このPJをやった理由？
仕事でコーティングする時、「一番よいデザイン・手順」より、「PJのデザインと合う、一番慣れる方法で早めに完成」が基本ですが、「難しくないが毎回同じようなものを作っている」「じっくり考えたらもっとよりデザインがあるはず」の感覚がよく出てきます。設計力を上げるため、個人PJをしようと考え、業務でよく作った「Filter/Sorter/Pagerなど検索UIのライブラリー化」をテーマとしました。

### Q: 困難は？
最初の困難は「全然始められない！」（笑）。使いやすいの形は想像出来なくて、最初は何時間も考えても何も出てこないです。それに対し、「とりあいず何か作ってみよう」と思って、デザインをなしにコーティングを始めました。結果：動けるものが出ってきましたが、明らかにライブラリーとして使えないものです。でもそのおかげて、使いやすいの形のイメージがだんだん出てきました。（最初のコードはsvn_xxxをcheckoutすれば見られます。）

途中出てきた困難は設計力不足です。コーティングすればするほど、「何故何時間もかけるのに、これくらいのコードしか出ってこない？」と気づいて、考えて見ると、「ロジックが複雑すぎて毎回ちゃんっと分析しなければならない」=>「どうすればDon't make me thinkingになれる？」、「複数のクラスを組み合わせるのは難しい」=>「どう設計すれば必ず使いやすい？」。対策としてオブジェクト指向デザインのレベルアップを決めました。[Agile Software Developmentの読書と練習](https://github.com/maxtangli/Personal/tree/master/2014.08_CSharp_EmployeePayment)をした後、デザインが前より良くなり、コーティングのスピードの改善が見られます。

### Q: 成長点は？
Unity・C#・設計力などの専門知識は無論、勉強の方法論はもっと重要だと思います。

今回の場合、以下の流れが幾つあります：

1. （「何の困難をあったか」より）「設計が複雑、スムーズに進めていない」＝＞「オブジェクト指向設計の手順が正しくない？」＝＞「オブジェクト指向設計の本を読む」＝＞「設計力のアップ」
2. 「簡単なことなのにC#でするのはこんなに難しい？」＝＞「ここで知らなかったC#のテクニックがあるかも」＝＞「C#の資料を読む」＝＞「なるほど、C#はparams keyword/ sturct / value object patternがある！」

まとめてみると、練習の中で、「（違和感より）問題点の気づき」＝＞「原因の分析より、足りない能力・知識の見つけ出し」＝＞「不足対しの勉強案の計画と実行」＝＞「効果の観測」のサイクルより、レベルアップのことができます。

（注：おおげさ・かたいすぎるかな？）

### Q: 自己評価？
今までやっとライブラリーの雰囲気が出ってくる、（インターフェイスが多いからかもしれませんが）、これからはEditorのサポート、UI系クラスのシンプル化などを注力します。

１４０時間もかけって、２０個くらいのクラスしか出てこないのは驚きました。使いやすい設計を出るのは難しい、もっとコード読みの勉強が必要だと考えます。

### Q: 検索UIは簡単な機能と思って、ライブラリー化の価値が本当にあるか？
（練習の目的もありますが）PJで、検索UIのためわざわざライブラリーを使うのは大袈裟と思いますが、もしNGUIのようなUIライブラリーの一部として提供すれば、使う人もあるでしょう。

### Q: 本格的にライブラリーを作る場合、どんな流れが良いと思う？
個人好み用ならどうでもいいと思うか、本格的な場合は事前計画をしっかりしないと、「必要ない・使いにくいものが出ってしまう」「膨大な時間の浪費」などのリスクが出やすいと思う。

本格的にライブラリーを作る場合、以下の流れが良いと思う：

1. 作る価値があるかの判断。現状・問題点の分析は無論、ユーザーいわゆるプログラマーたちに聞くのも必要だと思う。
2. 現状の調査。既に存在する関連ライブラリーを調べ、それぞれの持つ特性・利点・欠点をまとめて、今回のライブラリーの以来より良い特性・利点を決める。
3. demoの作成。最初は一番重要な特性だけを、質より開発速度重視の形で作り、自分またはユーザーにフィードバックをもらう。
4. betaの作成。今回は重要な特性をピックアップし、質重視の形で作り、出来たら公開してまたユーザーにフィードバックをもらう。
5. 改善。良いと思うユーザーが多いだったら、修正、改善、特性追加を進む。

### Q: このページに対する感想？

正直なところ、設計力・言語知識はまた不足、ライブラリーの作成にはまた入門レベルでもない、と考えています。
「このくらいのものをライブラリーと言うのは本当に大丈夫か」と悩んでいるところもありますが、個人のライブラリー作成の達人への道の一歩に対し、やっぱりとても良い経験になっているので、「ライブラリーと言ってもいいじゃないか」と考えています。

コードより、最初のuser storyの書き方は良い。
いつか広く使われているライブラリーのuser stroyを書ければいいな～
