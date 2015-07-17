using UnityEngine;

public class Card : MonoBehaviour {
	[SerializeField] private UILabel _label;
	[SerializeField] private UISprite _bgSprite;

	private static readonly Color[] _colors = { Color.white, Color.grey, Color.blue, Color.green };

	/// <summary>
	/// Inits card view based on its number in deck
	/// </summary>
	public void Init( int index ) {
		const int deckSize = 52;
		int cardNumber = index < deckSize ? (index + 1) : ((index % deckSize) + 1);
		if (cardNumber <= 0) {
			cardNumber = cardNumber % deckSize + deckSize;
		}
		_bgSprite.color = _colors[Mathf.Abs(index % _colors.Length)];
		_bgSprite.depth = index;
		_label.depth = index + 1;
		_label.text = cardNumber.ToString();
		name = index.ToString("D2");
	}
}
