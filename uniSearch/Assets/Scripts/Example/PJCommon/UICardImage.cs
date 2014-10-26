using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UI2DSprite))]
public class UICardImage : MonoBehaviour {
	[SerializeField]
	Card card;
	public Card Card {
		get {
			return card;
		}
		set {
			card = value;
			syncView();
		}
	}

	void syncView() {
		GetComponent<UI2DSprite> ().sprite2D = getCardImageSprite (card);
	}

	public static Sprite getCardImageSprite(Card card) {
		string filename = string.Format ("CardImages/{0}", card.Code.ToString("00"));
		var result = Resources.Load<Sprite> (filename);
		if (result == null) {
			throw new System.Exception(string.Format("Sprite file [{0}] not found.", filename));
		}
		return result;
	}

	public static UICardImage InstantiatePrefab(Card card) {
		string path = "UICardImage";
		var prefab = Resources.Load<UICardImage>(path);
		return UnityUtils.InstantiatePrefab(prefab, p => p.Card = card);
	}

	void OnValidate() {
		syncView ();
	}
}