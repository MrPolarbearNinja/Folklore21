using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;

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
    public LanePanel CurrentLane;     //The lane that had card played at last

    public int points;
    

    public ScoreSystem scoreSystem;

    public GameObject lanePrefab;
    public Transform lanePanel;



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
        StartGame();
    }

    public void StartGame()
    {
        deck.CreateDeck();
        deck.Shuffle();
        nextCard.ChangeCard(deck.DrawCard());
        currentCard.ChangeCard(deck.DrawCard());
    }

    //After a card has been placed down
    public void NextTurn()
    {
        scoreSystem.ScoreProccess();
        currentCard.ChangeCard(nextCard.card);
        nextCard.ChangeCard(deck.DrawCard());
        CheckIfAnyLaneIsLegal();
    }

    //checks if you can play the current card, if not, then game over
    public void CheckIfAnyLaneIsLegal()
    {
        foreach (LanePanel lane in lanePanels)
        {
            if (lane.IsLegal(currentCard.card))
            {
                return;
            }
        }
        ClearAllLanes();
    }
    

    public void ClearAllLanes()
    {
        foreach (LanePanel lane in lanePanels)
        {
            discard.cards.AddRange(lane.GetAllCards());
            lane.ClearLane();
        }
    }

    public void RestartGame()
    {
        discard.ClearDeck();
        deck.ClearDeck();
        ClearAllLanes();

        StartGame();
    }

    public void AddLane()
    {
        GameObject newLane = Instantiate(lanePrefab, lanePanel);
        lanePanels.Add(newLane.GetComponent<LanePanel>());
    }
}