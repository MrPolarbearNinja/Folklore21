using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LanePanel : MonoBehaviour
{
    public Transform cardContainer;     // Vertical Layout Group parent
    public TMP_Text valueText;          // Text at bottom showing total value
    public int score;                   // The score for this Lane
    public GameObject cardPrefab;       // prefab to spawn
    public GameObject ghostCardPrefab;  // prefab to spawn
    private List<Card> cards = new List<Card>();

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
        if (!IsLegal(card))
            return;
        GameObject newCard = Instantiate(ghostCardPrefab, cardContainer);
        ghostCard = newCard;
        newCard.GetComponent<FieldCard>().card = card;
        valueText.color = new Color(0.22f, 0.18f, 0.24f, 1f);
        valueText.outlineColor = new Color(0.22f, 0.18f, 0.24f, 1f);
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
        return (score + card.GetValue() <= 21 &&  !GameManager.Instance.gamePaused);
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

    void UpdateValueDisplay(int extra = 0)
    {
        int total = 0;
        int aceCount = 0;

        // Blackjack-style ace logic
        foreach (var c in cards)
        {
            int value = c.GetValue();

            total += value;

            if (c.rank == Rank.Ace)
                aceCount++;
        }

        // Handle Ace = 11 optimization
        while (aceCount > 0 && total + 10 <= 21)
        {
            total += 10;
            aceCount--;
        }

        
        if (extra != 0)
            valueText.text = (total + extra).ToString();
        else
            valueText.text = total.ToString();
        score = total;
    }
}