using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingCell : MonoBehaviour
{
    public Card holdingCard;
    public bool empty = true;

    public void ChangeCard()
    {

        if (empty)
        {
            holdingCard = GameManager.Instance.currentCard.card;
            GameManager.Instance.NextTurn();
            empty = false;
        }
        else
        {
            Card tempCard = holdingCard;
            holdingCard = GameManager.Instance.currentCard.card;
            GameManager.Instance.currentCard.card = tempCard;
        }

        
    }
}
