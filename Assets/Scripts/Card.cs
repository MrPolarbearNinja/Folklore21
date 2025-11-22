using UnityEngine;

public enum CardType
{
    Normal,
    Magic
}
public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades,
    Explotion
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
    King = 13,
    Magic = 0
}

[System.Serializable]
public class Card
{
    public Suit suit;
    public Rank rank;
    public CardType cardType;

    public ScriptableObject magicEffect;

    public Card(Suit suit, Rank rank, CardType type = CardType.Normal, ScriptableObject effect = null)
    {
        this.suit = suit;
        this.rank = rank;
        this.cardType = type;
        this.magicEffect = effect;
    }

    public int GetValue()
    {
        return (int)rank;
    }

    public void ActivateMagic()
    {
        if (cardType != CardType.Magic) return;
        if (magicEffect == null) return;

        (magicEffect as IMagicEffect)?.Activate();
    }

    public override string ToString()
    {
        return $"{rank} of {suit}";
    }
}