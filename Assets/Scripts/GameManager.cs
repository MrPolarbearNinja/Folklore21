using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum Score
{
    Normal_21 = 100,
    Large_21 = 150,
    Very_Large_21 = 250,
    BlackJack = 200,
    Pair = 150,
    Three_of_a_kind = 300,
    Four_of_a_kind = 900,
    Full_House = 900,
    Straight = 300,
    Five_of_a_Kind = 300
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Deck deck;
    public Deck discard;
    public FieldCard currentCard;
    public FieldCard nextCard;

    public int points;
    public TMP_Text scoreText;

    private int comboCounter;
    private bool comboFlag;



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
        deck.CreateDeck();
        deck.Shuffle();
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
        //Check if all lanes have the same score
        if (lanePanels.All(c => c.score == lanePanels[0].score))
        {
            AddPoints(Score.Five_of_a_Kind);
            ClearAllLanes();
            return;
        }

        //Check if all lanes are in sequence
        List<int> scores = lanePanels.Select(c => c.score).ToList();

        int min = scores.Min();
        int max = scores.Max();

        if (scores.Distinct().Count() == scores.Count && (max - min + 1) == scores.Count)
        {
            AddPoints(Score.Straight);
            ClearAllLanes();
            return;
        }


        //check each lane
        foreach (LanePanel lane in lanePanels)
        {
            if (lane.score == 21)
            {
                AddPoints(Score.Normal_21);
                discard.cards.AddRange(lane.GetAllCards());
                lane.ClearLane();
                return;
            }
        }
        comboFlag = false;
    }

    public void ClearAllLanes()
    {
        foreach (LanePanel lane in lanePanels)
        {
            discard.cards.AddRange(lane.GetAllCards());
            lane.ClearLane();
        }
    }

    public void AddPoints(Score score)
    {
        if (!comboFlag)
            comboCounter = 0;

        comboCounter++;
        int multiplyer = 1;

        if (comboCounter == 1)
            multiplyer = 1;
        else if (comboCounter == 2)
            multiplyer = 2;
        else if (comboCounter == 3)
            multiplyer = 3;
        else if (comboCounter == 4)
            multiplyer = 5;
        else if (comboCounter == 4)
            multiplyer = 10;

        points += (int)score * multiplyer;
        scoreText.text = "Points: " + points;
        
        comboFlag = true;
    }

}