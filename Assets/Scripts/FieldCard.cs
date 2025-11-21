using UnityEngine;
using UnityEngine.UI;

public class FieldCard : MonoBehaviour
{
    public Image cardImage;                 // UI Image for the card
    public Card card;

    public CardSpriteDatabase spriteDatabase;

    void Start()
    {
        UpdateCardVisual();
    }

    public void ChangeCard(Card newCard)
    {
        card = newCard;
        UpdateCardVisual();
    }

    public void UpdateCardVisual()
    {
        if (spriteDatabase != null)
        {
            cardImage.sprite = spriteDatabase.GetSprite(card.suit, card.rank);
        }
    }
}
