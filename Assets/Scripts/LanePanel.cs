using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LanePanel : MonoBehaviour
{
    public Transform cardContainer;     // Vertical Layout Group parent
    public TMP_Text valueText;          // Text at bottom showing total value
    public int score;                   // The score for this Lane
    public GameObject cardPrefab;        //prefab to spawn
    private List<Card> cards = new List<Card>();

    void Start()
    {
        UpdateValueDisplay();
    }

    public void LaneClick()
    {
        AddCard(GameManager.Instance.currentCard.card);
    }

    public void AddCard(Card card)
    {
        cards.Add(card);

        GameObject newCard = Instantiate(cardPrefab, cardContainer);
        newCard.GetComponent<FieldCard>().card = card;
        UpdateValueDisplay();
        GameManager.Instance.NextTurn();
    }

    public void RemoveCard(Card card, GameObject cardVisual)
    {
        cards.Remove(card);
        Destroy(cardVisual);

        UpdateValueDisplay();
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