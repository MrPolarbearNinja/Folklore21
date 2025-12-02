using UnityEngine;
using UnityEngine.UI;

public class FieldCard : MonoBehaviour
{
    public Image cardImage;                 // UI Image for the card
    public Card card;
    public ParticleSystem destroyParticles;

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


    void OnDestroy()
    {
        if (destroyParticles != null)
        {

            Vector3 spawnPos = transform.GetChild(0).position;

            Vector3 dirToCamera = (Camera.main.transform.position - transform.position).normalized;
            spawnPos += dirToCamera * 0.05f;

            ParticleSystem ps = Instantiate(destroyParticles, spawnPos, Quaternion.identity);

            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);

        }
        
    }
}
