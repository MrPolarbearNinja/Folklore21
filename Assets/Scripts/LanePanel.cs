using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;

public class LanePanel : MonoBehaviour, IDropHandler
{
    public Transform cardContainer;     // Vertical Layout Group parent
    public TMP_Text valueText;          // Text at bottom showing total value
    public int score;                   // The score for this Lane
    public GameObject cardPrefab;       // prefab to spawn
    public GameObject ghostCardPrefab;  // prefab to spawn
    public List<Card> cards = new List<Card>();

    public GameObject scorePopUp;

    public TMP_Text scoreTypePopUpText;
    public TMP_Text scoreValueText;

    private GameObject ghostCard;
           

    void Start()
    {
        UpdateValueDisplay();
    }

    public void LaneClick()
    {
        AddCard(GameManager.Instance.currentCard.card);
    }
    public void LaneHover()
    {
        SpawnGhostCard(GameManager.Instance.currentCard.card);
    }
    public void LaneUnHover()
    {
        DeSpawnGhostCard();
    }

    public void SpawnGhostCard(Card card)
    {
        if (GameManager.Instance.gamePaused)
            return;
        if (IsLegal(card))
        {
            GameObject newCard = Instantiate(ghostCardPrefab, cardContainer);
            ghostCard = newCard;
            newCard.GetComponent<FieldCard>().card = card;
            valueText.color = new Color(0, 0.7f, 0, 1);
            valueText.outlineColor = Color.black;
        }
        else
        {
            valueText.color = new Color(0.7f, 0, 0, 1);
            valueText.outlineColor = Color.red;
        }
        
        UpdateValueDisplay(card.GetValue());
    }
    public void DeSpawnGhostCard()
    {
        Destroy(ghostCard);
        valueText.color = Color.white;
        valueText.outlineColor = Color.white;
        UpdateValueDisplay();
    }

    public bool IsLegal(Card card)
    {
        if (GameManager.Instance.gamePaused)
            return false;

        int aceCount = 0;
        int total = 0;

        foreach (Card c in cards)
        {
            total += c.GetValue();
            if (c.rank == Rank.Ace)
                aceCount++;

        }
        return ((total + card.GetValue()) <= 21 );
    }

    public void AddCard(Card card)
    {
        if (!IsLegal(card))
            return;
        DeSpawnGhostCard();
        GameManager.Instance.currentCard.GetComponent<Animator>().Play("Card_NextCard", 0, 0f);
        GameManager.Instance.nextCard.GetComponent<Animator>().Play("Card_DrawCard", 0, 0f);
        cards.Add(card);
        GameObject newCard = Instantiate(cardPrefab, cardContainer);
        newCard.GetComponent<FieldCard>().card = card;
        UpdateValueDisplay();
        GameManager.Instance.CurrentLane = this;
        card.ActivateMagic();

        GameManager.Instance.NextTurn();
    }

    public void RemoveCard(Card card, GameObject cardVisual)
    {
        cards.Remove(card);
        Destroy(cardVisual);

        UpdateValueDisplay();
    }

    public void ClearLane()
    {
        GameManager.Instance.discard.cards.AddRange(cards);
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        cards.Clear();

        UpdateValueDisplay();
    }

    public List<Card> GetAllCards()
    {
        return cards;
    }

    public void PopUpScore(Score scoretype)
    {
        scorePopUp.SetActive(true);
        this.gameObject.GetComponent<Animator>().Play("Lane_PopUp",0,0);

        if (GameManager.Instance.scoreSystem.comboCounter > 1)
        {
            scoreTypePopUpText.text = scoretype.ToString().Replace("_", " ") + " COMBO";
        }
        else
        {
            scoreTypePopUpText.text = scoretype.ToString().Replace("_", " ") ;
        }

        int multiplyer = 1;
        int comboCounter = GameManager.Instance.scoreSystem.comboCounter;

        if (comboCounter == 1)
            multiplyer = 1;
        else if (comboCounter == 2)
            multiplyer = 2;
        else if (comboCounter == 3)
            multiplyer = 3;
        else if (comboCounter == 4)
            multiplyer = 5;
        else if (comboCounter == 4)
            multiplyer = 10;

        int target = (int)scoretype * multiplyer;
        // Reset scale
        scoreValueText.transform.localScale = Vector3.one;

        // Number tween
        int currentValue = 0;
        DOTween.To(() => currentValue, x =>
        {
            currentValue = x;
            scoreValueText.text = currentValue.ToString();
        },
        target, 0.6f).SetEase(Ease.OutQuad);

        // Bunch / punch scale
        scoreValueText.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0), 0.4f, 10, 1f);
    }

    void UpdateValueDisplay(int extra = 0)
    {
        int total = 0;
        int aceCount = 0;

        total += extra;
        foreach (var c in cards)
        {
            int value = c.GetValue();

            total += value;

            if (c.rank == Rank.Ace)
                aceCount++;
        }

        while (aceCount > 0 && total + 10 <= 21)
        {
            if (total < 21)
                total += 10;
            aceCount--;
        }

        

        valueText.text = total.ToString();
        score = total;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Try to get the draggable card script from the dropped object
        FieldCard card = eventData.pointerDrag?.GetComponent<FieldCard>();

        if (card != null)
        {
            AddCard(card.card);
        }
    }
}