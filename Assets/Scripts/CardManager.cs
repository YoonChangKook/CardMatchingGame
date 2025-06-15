using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Header("Card Prefab & Layout")]
    public GameObject cardPrefab;
    public Transform cardsRoot;

    [Header("Card Sprites")]
    public Sprite[] uniqueCardSprites; // 원본 전체 카드들
    public Sprite cardBackSprite;      // 공통 회색 뒷면 이미지

    private List<CardController> spawnedCards = new List<CardController>();
    private const int pairCount = 10;  // 10쌍 = 20장

    private List<CardController> cards = new List<CardController>();
    private CardController firstCard, secondCard;

    public GameManager gameManager;

    public bool canToggle = false; // false: 클릭 금지, true: 클릭 허용

    void Start()
    {
        //SpawnCards();
    }

    public void SpawnCards()
    {
        ClearCards();

        // 1. uniqueCardSprites 중 랜덤으로 10장 선택
        List<Sprite> available = new List<Sprite>(uniqueCardSprites);
        Shuffle(available);

        List<(Sprite sprite, int id)> cardPairs = new List<(Sprite, int)>();

        for (int i = 0; i < pairCount && i < available.Count; i++)
        {
            Sprite selected = available[i];
            cardPairs.Add((selected, i));
            cardPairs.Add((selected, i)); // 짝 추가
        }

        // 2. 셔플하여 랜덤 배치
        Shuffle(cardPairs);

        // 3. 카드 인스턴스 생성
        foreach (var data in cardPairs)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardsRoot);
            CardController controller = cardObj.GetComponent<CardController>();

            controller.frontImage = cardObj.transform.Find("Front").GetComponent<Image>();
            controller.backImage = cardObj.transform.Find("Back").GetComponent<Image>();

            controller.SetCard(data.sprite, data.id, cardBackSprite);

            spawnedCards.Add(controller);

            Button btn = cardObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(controller.Toggle);

            RegisterCard(controller);
        }

        AdjustGridLayout();
    }

    public void ClearCards()
    {
        foreach (Transform child in cardsRoot)
        {
            Destroy(child.gameObject);
        }
        spawnedCards.Clear();
        cards.Clear();
    }

    public List<CardController> GetAllCards()
    {
        return spawnedCards;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void AdjustGridLayout()
    {
        GridLayoutGroup grid = cardsRoot.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 4;
        }
    }

    public void HideAllCards()
    {
        foreach (CardController card in spawnedCards)
        {
            card.ShowBack();
        }

        canToggle = true; // Toggle 허용
    }

    public bool CanToggleCards()
    {
        return canToggle;
    }

    public void OnCardSelected(CardController selected)
    {
        if (firstCard == null)
        {
            firstCard = selected;
            firstCard.ShowFront();
        }
        else if (secondCard == null && selected != firstCard)
        {
            secondCard = selected;
            secondCard.ShowFront();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canToggle = false;
        yield return new WaitForSeconds(0.5f); // 연출용 지연 (바로 안 뒤집어지게)

        if (firstCard.GetCardId() == secondCard.GetCardId())
        {
            firstCard.Lock();
            secondCard.Lock();
        }
        else
        {
            firstCard.ShowBack();
            secondCard.ShowBack();
        }

        firstCard = secondCard = null;
        canToggle = true;
    }

    public void RegisterCard(CardController card)
    {
        cards.Add(card);
    }

    public bool AreAllCardsMatched()
    {
        return cards.Count > 0 && cards.All(c => c.isMatched);
    }

    public void ShowScore()
    {
        int matchedCount = cards.Count(card => card.isMatched);
        Debug.Log("Matched Count: " + matchedCount + ", Total cards: " + cards.Count + ", All Matched: " + cards.All(c => c.isMatched));
    }
}