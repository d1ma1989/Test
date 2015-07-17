using UnityEngine;

using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour {
	[SerializeField] private UISprite _bgSprite;
	[SerializeField] private UISprite _innerBgSprite;
	[SerializeField] private UIScrollView _scrollView;
	[SerializeField] private UIWrapContent _uiWrapContent;
	[SerializeField] private Transform _poolGOTransform;

	private int _screenWidth;
	private int _screenHeight;

	private GameObject _cardPrefab;

	private static readonly List<GameObject> _activeCardsObjects = new List<GameObject>();
	private static readonly List<GameObject> _cardsPool = new List<GameObject>();

	private void Start() {
		_cardPrefab = Resources.Load<GameObject>("Card");
		_uiWrapContent.onInitializeItem += InitCard;
	}

	/// <summary>
	/// Reinits wrapped card with new index
	/// </summary>
	private static void InitCard(GameObject go, int wrapIndex, int realIndex) {
		go.GetComponent<Card>().Init(realIndex);
	}

	// Checking resolution changes
	private void Update() {
		if (_screenWidth == Screen.width && _screenHeight == Screen.height) {
			return;
		}
		_screenHeight = Screen.height;
		_screenWidth = Screen.width;

		OnResolutionChanged();
	}

	/// <summary>
	/// Resets size of widgets to match the new screen resolution
	/// </summary>
	private void OnResolutionChanged() {
		_bgSprite.ResetAnchors();
		_innerBgSprite.width = _bgSprite.width / 2;
		_scrollView.panel.ResetAnchors();
		_scrollView.panel.Update();
		ShowCards();
	}

	/// <summary>
	/// Shows needed amount of cards for current screen width
	/// </summary>
	private void ShowCards() {
		PoolCards();
		int cardsAmount = (int)( _scrollView.panel.width / _uiWrapContent.itemSize ) + 1;
		for (int i = 0; i <= cardsAmount; i++) {
			Transform cardTransform = GetCardFromPool().transform;
			cardTransform.parent = _uiWrapContent.transform;
			cardTransform.localPosition = Vector3.zero;
			cardTransform.localScale = Vector3.one;
			cardTransform.GetComponent<Card>().Init(i);
		}
		_uiWrapContent.SortAlphabetically();
		_scrollView.ResetPosition();
	}

	/// <summary>
	/// Gets card gameobject from pool or instantiates one if it's empty
	/// </summary>
	private GameObject GetCardFromPool() {
		GameObject cardGO;
		if (_cardsPool.Any()) {
			cardGO = _cardsPool[0];
			_cardsPool.Remove(cardGO);
		} else {
			cardGO = Instantiate(_cardPrefab);
		}
		cardGO.SetActive(true);
		_activeCardsObjects.Add(cardGO);
		return cardGO;
	}

	/// <summary>
	/// Adds all active card objects to pool
	/// </summary>
	private void PoolCards() {
		foreach (GameObject cardGO in _activeCardsObjects) {
			cardGO.SetActive(false);
			cardGO.transform.parent = _poolGOTransform;
			_cardsPool.Add(cardGO);
		}
		_activeCardsObjects.Clear();
	}
}
