using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Deck deck;
    public FieldCard currentCard;
    public FieldCard nextCard;

    public int points;
    public TMP_Text scoreText;


    public List<LanePanel> lanePanels = new List<LanePanel>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        nextCard.ChangeCard(deck.DrawCard());
        currentCard.ChangeCard(deck.DrawCard());
    }

    //After a card has been placed down
    public void NextTurn()
    {
        currentCard.ChangeCard(nextCard.card);
        nextCard.ChangeCard(deck.DrawCard());
    }
}