using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public enum Score
{
    Normal_21 = 100,
    Large_21 = 150,
    Very_Large_21 = 250,
    BlackJack = 200,
    Pair = 150,
    Three_of_a_kind = 300,
    Four_of_a_kind = 900,
    Full_House = 900
}

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
        ScoreProccess();
        currentCard.ChangeCard(nextCard.card);
        nextCard.ChangeCard(deck.DrawCard());
    }

    public void ScoreProccess()
    {
        //Check all lanes
        foreach (LanePanel lane in lanePanels)
        {
            
        }
        //check each lane
        foreach (LanePanel lane in lanePanels)
        {
            if (lane.score == 21)
            {
                points += (int)Score.Normal_21;
                lane.ClearLane();
            }

        }
        scoreText.text = "Points: " + points;
    }

    public void ClearAllLanes()
    {
        foreach (LanePanel lane in lanePanels)
        {
            lane.ClearLane();
        }
    }

}