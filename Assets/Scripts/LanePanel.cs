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
        GameObject newCard = Instantiate(ghostCardPrefab, cardContainer);
        ghostCard = newCard;
        newCard.GetComponent<FieldCard>().card = card;
    }
    public void DeSpawnGhostCard()
    {
        Destroy(ghostCard);
    }

    public void AddCard(Card card)
    {
        DeSpawnGhostCard();
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

    void UpdateValueDisplay()
    {
        int total = 0;
        int aceCount = 0;

        // Blackjack-style ace logic
        foreach (var c in cards)
        {
            int value = c.GetValue();

            if (value > 10)
                value = 10;

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

        valueText.text = total.ToString();
        score = total;
    }
}