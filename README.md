uniSearch(developing)
=========

A practice aiming at a library to simplify Unity UI filter/sorter/pager workflow.

## バックグラウンド

- 検索UIとは：Filt/Sort/Range/Pageなどの検索条件に関するUI機能
- ゲーム中、検索UIはよくある機能。
- Console・Web・Mobileより、UIの形が違いたりしている。
- ソーシャルゲームの場合、負荷軽減のためロカール検索は普通で、リモート検索もある。

## 問題点

検索UIを作るのは難しくないが、Button・DialogBoxのような標準UI概念がないので：

- 同じ機能を何度も作らなければならない。
- 人により作り方が違い。
- 細かいところが考えていない恐れがあり。
- 工数のため、一番よく使わる検索オプションしか提供しない傾向。

## 解決案

検索UIライブラリーを用意し、検索UI機能を標準化し、工数を出来るだけ軽減する。

そのUIライブラリーは、主に以下３つの部分があります：
1. 検索用データのインターフェイス
2. 検索用データより、ロカール検索処理用のクラス
3. 検索用データを提供するUIコンポーネント

### Controller Code

UniUISearcher uiSearcher; // later descripted
UniSearcher<CardData> searcher;
void Start() {
 IEnumerable<CardData> fullDatas = getFullDatas();
 
// declare search condition and execution
UniSearcher<CardData> searcher = new UniSearcher<CardData>(
// data collection to be searched
fullDatas,
// filter decleration
'レアリティー', // displayed filter title
 'Nレア' , x => x.rarity = N, // displayed filter option, and its execution
 'Rレア' , x => x.rarity = R,
 'SRレア', x => x.rarity = SR,
'攻撃力',
 '<1000' , x => x.atk < 1000,
 '1000-1999', x => x.atk >= 1000 && x.atk < 2000
 '2000~' x => x.atk >= 2000,
....
// sorter declartion
'レアリティー順', x.rarity, SortType.ASC // displayed sorter title, and its execution
'攻撃力順', x.atk, SortType.ASC
...
);

 uiSearcher.onInteraction += onUISearcher;
 uiSearcher.searcherData = searcher.DefaultSearcherData;
 onUISearcher();
}
void onUISearcher() {
 searcher.doSearch(uiSearcher.searcherData, updateView);
}
void updateView(UniSearchResult result) {
 // update scene's objects using result.datas/.numRecordAll/.numRecordAfterFilt/.numRecordAfterRange etc.
 ....
}

### UniUISearcherの説明

TODO

UniUIsearcher: provide filter/sorter/ranger data.
UniUIFilter: provide filter data.
 UniUIOptionListFilter : provider filter data by a list of UIOption.
  MyPjFilterPrefab : Project's standard filter prefab.
UniUISorter
UniUIRanger: provide ranger data.
 UniUIPager: provide ranger data by pager data.
  NGUI4Button1LabelPager: provide pager data by 4 UIButton and 1 UILabel.  
   MyPjPagerPrefab : Project's standard pager prefab.
   
## Q&A

Q: なぜこんなことをやったか？
A: 仕事でコーティングする時、「一番よいデザイン・手順」より、「PJのデザインと合う、一番慣れる方法で早めに完成」が基本ですが、「難しくないが毎回同じようなものを作っている」「じっくり考えたらもっとよりデザインがあるはず」の感覚がよく出てきます。設計力を上げるため、個人PJをしようと考え、業務でよく作った「Filter/Sorter/Pagerなど検索UIのライブラリー化」をテーマとしました。

Q: 何の困難をあったか？
A: TODO

Q: 成長点は？
A: TODO

Q: 検索UIは簡単な機能と思って、ライブラリー化の価値が本当にあるか？
A: （本来は練習の目的ですが）PJで、検索UIのためわざわざライブラリーを使うのは大袈裟と思いますが、もしNGUIのようなUIライブラリーの一部として提供すれば、使う人もあるでしょう
。

Q: 本格的にライブラリーを作る場合、どんな流れが良いと思う？
A: 

個人好み用ならどうでもいいと思うか、本格的な場合は事前計画をしっかりしないと、
「必要ない・使いにくいものが出ってしまう」「膨大な時間の浪費」などのリスクが出やすいと思う。

本格的にライブラリーを作る場合、以下の流れが良いと思う：

1. 作る価値があるかの判断。現状・問題点の分析は無論、ユーザーいわゆるプログラマーたちに聞くのも必要だと思う。
2. 現状の調査。既に存在する関連ライブラリーを調べ、それぞれの持つ特性・利点・欠点をまとめて、今回のライブラリーの以来より良い特性・利点を決める。
3. demoの作成。最初は一番重要な特性だけを、質より開発速度重視の形で作り、自分またはユーザーにフィードバックをもらう。
4. betaの作成。今回は重要な特性をピックアップし、質重視の形で作り、出来たら公開してまたユーザーにフィードバックをもらう。
5. 改善。良いと思うユーザーが多いだったら、修正、改善、特性追加を進む。