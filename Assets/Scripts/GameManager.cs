using UnityEngine;
using System.Collections.Generic;
using Fungus;

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
    public Flowchart targetFlowchart;

    public bool gamePaused;

    public int points;
    public int coin;
    public int adventureStage = 0;

    private int tutorialStep = 1;
    public ScoreSystem scoreSystem;

    public GameObject lanePrefab;
    public Transform lanePanel;

    private bool isInTutorial;

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
        StartTutorial();
    }

    public void StartTutorial()
    {
        isInTutorial = true;
        nextCard.ChangeCard(deck.DrawCard(true));
        currentCard.ChangeCard(deck.DrawCard(true));
    }
    public void EndTutorial()
    {
        isInTutorial = false;
        gamePaused = false;
        ProgressAdventure();
    }

    public void StartGame()
    {
        Debug.Log("Game Start");
        deck.CreateDeck();
        deck.Shuffle();
        discard.ClearDeck();
        nextCard.ChangeCard(deck.DrawCard());
        currentCard.ChangeCard(deck.DrawCard());
        currentCard.GetComponent<Animator>().Play("Card_NextCard", 0, 0f);
        Instance.nextCard.GetComponent<Animator>().Play("Card_DrawCard", 0, 0f);
    }

    //After a card has been placed down
    public void NextTurn()
    {
        if (isInTutorial)
        {
            if (tutorialStep == 5)
            {
                targetFlowchart.StopBlock("Step5");
                gamePaused = true;
            }
            else
            {
                tutorialStep += 1;
                targetFlowchart.ExecuteBlock("Step" + tutorialStep);
            }
        }
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
        if (isInTutorial)
        {
            ProgressTutorial();
            return;
        }
        //currentCard.GetComponent<Animator>().Play("Card_FallDown", 0, 0f);
        //Instance.nextCard.GetComponent<Animator>().Play("Card_FallDown", 0, 0f);

        GameOver();
    }

    public void GameOver()
    {
        adventureStage = 0;
        Flowchart.BroadcastFungusMessage("GameOver");
    }
    //At the store when you click continue
    public void Continue()
    {
        Flowchart.BroadcastFungusMessage("Continue");
    }

    //When you finish the deck
    public void ProgressAdventure()
    {
        if (adventureStage >= 4)
        {
            VictoryTrigger();
            return;
        }
        string message = "Adventure" + adventureStage.ToString();
        Flowchart.BroadcastFungusMessage(message);
        adventureStage += 1;
    }

    public void VictoryTrigger()
    {
        Flowchart.BroadcastFungusMessage("Ending");
    }


    public void ClearAllLanes()
    {
        foreach (LanePanel lane in lanePanels)
        {

            lane.ClearLane();
        }
    }

    public void ProgressTutorial()
    {
        Fungus.Flowchart.BroadcastFungusMessage("Introduction");
    }
    public void ClearTutorialBoard()
    {
        currentCard.GetComponent<Animator>().Play("Card_FallDown", 0, 0f);
        Instance.nextCard.GetComponent<Animator>().Play("Card_FallDown", 0, 0f);
        ClearAllLanes();
    }

    public void RestartGame()
    {
        ClearAllLanes();
        discard.AddCard(nextCard.card); 
        discard.AddCard(currentCard.card);
        deck.ResicleFromDiscard();
        deck.Shuffle();
        nextCard.ChangeCard(deck.DrawCard());
        currentCard.ChangeCard(deck.DrawCard());
        currentCard.GetComponent<Animator>().Play("Card_NextCard", 0, 0f);
        Instance.nextCard.GetComponent<Animator>().Play("Card_DrawCard", 0, 0f);
        scoreSystem.RestartScore();
        ProgressAdventure();
    }

    public void AddLane()
    {
        GameObject newLane = Instantiate(lanePrefab, lanePanel);
        lanePanels.Add(newLane.GetComponent<LanePanel>());
    }
}