using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;

    public bool isMatched = false;
    private bool isFront = true;
    private int cardId; // 동일 페어를 구별하는 ID

    private CardManager cardManager;

    void Start()
    {
        // 카드 생성 시 CardManager 찾기
        cardManager = FindObjectOfType<CardManager>();
    }

    public void SetCard(Sprite frontSprite, int id, Sprite backSprite)
    {
        cardId = id;
        frontImage.sprite = frontSprite;
        backImage.sprite = backSprite;
        ShowFront();
    }

    public void ShowFront()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isFront = true;
    }

    public void ShowBack()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFront = false;
    }

    public void Toggle()
    {
        if (cardManager == null || !cardManager.CanToggleCards() || isMatched)
            return;

        cardManager.OnCardSelected(this);
    }

    public void Lock() {
        isMatched = true;
        //cardManager.ShowScore();
    }

    public bool IsFront() => isFront;

    public int GetCardId() => cardId;
}