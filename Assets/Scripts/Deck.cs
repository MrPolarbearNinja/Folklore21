using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Deck : MonoBehaviour
{
    public List<Card> cards;
    public TMP_Text cardsText;

    public ScriptableObject explosionEffect;
    public AudioClip drawSound;

    public void Start()
    {
        if (cardsText != null)
            cardsText.text = cards.Count.ToString();
    }

    // Create a standard 52-card deck
    public void CreateDeck()
    {
        Suit[] validSuits = { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
        Rank[] validRanks =
        {
            Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
            Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten,
            Rank.Jack, Rank.Queen, Rank.King, Rank.Ace
        };
        cards = new List<Card>();

        foreach (var suit in validSuits)
        {
            foreach (var rank in validRanks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
        if (cardsText != null)
            cardsText.text = cards.Count.ToString();

        //temp add 8 explosions to the deck
        cards.Add(new Card(Suit.Hero, Rank.Magic, CardType.Magic, explosionEffect));
        cards.Add(new Card(Suit.Hero, Rank.Magic, CardType.Magic, explosionEffect));
        cards.Add(new Card(Suit.Hero, Rank.Magic, CardType.Magic, explosionEffect));
        cards.Add(new Card(Suit.Hero, Rank.Magic, CardType.Magic, explosionEffect));

    }

    public void ResicleFromDiscard()
    {
        cards.AddRange(GameManager.Instance.discard.cards);
        GameManager.Instance.discard.ClearDeck();
        if (cardsText != null)
            cardsText.text = cards.Count.ToString();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
        if (cardsText != null)
            cardsText.text = cards.Count.ToString();
    }
    public void RemoveCard(Card card)
    {
        cards.Remove(card);
        if (cardsText != null)
            cardsText.text = cards.Count.ToString();
    }
    public void ClearDeck()
    {
        cards.Clear();
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
    public Card DrawCard(bool hideSound = false)
    {
        if (!hideSound)
            GetComponent<AudioSource>().PlayOneShot(drawSound);

        if (cards.Count == 0)
        {
            Debug.Log("Deck is empty, regenerating...");
            ResicleFromDiscard();
            GameManager.Instance.ProgressAdventure();
            Shuffle();
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        if (cardsText.text != null)
            cardsText.text = cards.Count.ToString();
        return drawnCard;
    }

    // Cards left in the deck
    public int CardsRemaining()
    {
        return cards.Count;
    }
}