using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardSpriteDatabase", menuName = "Cards/Sprite Database")]
public class CardSpriteDatabase : ScriptableObject
{
    public List<CardSpriteEntry> sprites = new List<CardSpriteEntry>();

    [System.Serializable]
    public class CardSpriteEntry
    {
        public Suit suit;
        public Rank rank;
        public Sprite sprite;
    }

    public Sprite GetSprite(Suit suit, Rank rank)
    {
        foreach (var entry in sprites)
        {
            if (entry.suit == suit && entry.rank == rank)
                return entry.sprite;
        }

        Debug.LogWarning($"Sprite not found for: {rank} of {suit}");
        return null;
    }
}