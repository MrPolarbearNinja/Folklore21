using UnityEngine;

public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}

[System.Serializable]
public class Card
{
    public Suit suit;
    public Rank rank;

    public Card(Suit suit, Rank rank)
    {
        this.suit = suit;
        this.rank = rank;
    }

    public int GetValue()
    {
        return (int)rank;
    }

    public override string ToString()
    {
        return $"{rank} of {suit}";
    }
}