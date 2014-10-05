using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CardColor{HEART=0,SPADE,CLUB,DIAMOND}
public static class CardColorExtensions {
	public static bool IsRed(this CardColor color) {
		return color==CardColor.HEART || color==CardColor.DIAMOND;
	}
	public static bool IsBlack(this CardColor color) {
		return !color.IsRed();
	}
}

public static class CardUtil {
	public static bool ValidCode(int code) {
		return 1 <= code && code <= 52;
	}
	public static bool ValidPoint(int point) {
		return 1 <= point && point <= 13;
	}
	public static int ColorPointToCode(CardColor color, int point) {
		if (!ValidPoint (point)) {
			throw new System.ArgumentException ();
		}
		return ColorOffsets[color] + point - 1;
	}
	public static CardColor CodeToCardColor(int code) {
		if (!ValidCode (code)) {
			throw new System.ArgumentException ();
		}
		return ColorOffsets.Where(x => {
			int dis = code - x.Value;
			return 0 <= dis && dis < 13;
		}).Single().Key;
	}
	public static int CodeToPoint(int code) {
		if (!ValidCode (code)) {
			throw new System.ArgumentException ();
		}
		return code - ColorOffsets[CodeToCardColor(code)] + 1;
	}
	static IDictionary<CardColor, int> ColorOffsets {
		get {
			return new Dictionary<CardColor, int> {
				{CardColor.HEART, 1},
				{CardColor.SPADE, 14},
				{CardColor.CLUB, 27},
				{CardColor.DIAMOND, 40},
			};
		}
	}
}

// poker card exclude jokers.
[System.Serializable]
public class Card {
	[SerializeField]
	CardColor color;
	public CardColor Color {
		get {
			return color;
		}
	}

	[SerializeField]
	int point;
	public int Point {
		get {
			return point;
		}
	}
		
	public Card(CardColor color, int point) {
		this.point = point;
		this.color = color;
	}
		
	public string PointText {
		get {
			switch (Point) {
			case 1: return "A";
			case 11: return "J";
			case 12: return "Q";
			case 13: return "K";
			default: return Point.ToString();
			}
		}
	}

	public int Code {
		get {
			return CardUtil.ColorPointToCode(Color, Point);
		}
	}

	public override string ToString ()
	{
		return string.Format ("[{0}{1}]", Color.ToString(), PointText);
	}
}