using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingCell : MonoBehaviour
{
    public FieldCard holdingCard;
    public bool empty = true;

    public void ChangeCard()
    {
        if (GameManager.Instance.currentCard.card.rank == 0)
            return;

        if (empty)
        {
            holdingCard.gameObject.SetActive(true);
            holdingCard.ChangeCard(GameManager.Instance.currentCard.card);
            GameManager.Instance.NextTurn();
            empty = false;
        }
        else
        {
            Card tempCard = holdingCard.card;
            holdingCard.ChangeCard(GameManager.Instance.currentCard.card);
            GameManager.Instance.currentCard.ChangeCard(tempCard);
        }

        
    }
}
