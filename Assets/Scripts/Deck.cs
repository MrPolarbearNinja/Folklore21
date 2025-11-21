using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> cards;

    private void Awake()
    {
        CreateDeck();
        Shuffle();
    }

    // Create a standard 52-card deck
    public void CreateDeck()
    {
        cards = new List<Card>();

        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    // Fisher–Yates shuffle
    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rand = Random.Range(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[rand];
            cards[rand] = temp;
        }
    }

    // Draw the top card from the deck
    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("Deck is empty, regenerating...");
            CreateDeck();
            Shuffle();
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }

    // Cards left in the deck
    public int CardsRemaining()
    {
        return cards.Count;
    }
}