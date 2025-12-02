using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour
{
    public List<GameObject> laneCovers = new List<GameObject>();

    public TMP_Text fc_text;
    public TMP_Text lane_cost;

    public Deck shopDeck;

    public void Start()
    {
        EnableShop();
    }

    public void EnableShop()
    {
        UpdateCoins();
        DisplayLanes();
    }

    public void BuyLane()
    {
        int laneCounts = GameManager.Instance.lanePanels.Count;
        if (laneCounts < 3)
        {
            GameManager.Instance.AddLane();
            laneCovers[laneCounts].GetComponent<Animator>().Play("LaneCover_EnableLane", 0, 0f);
        }
        if (laneCounts == 3)
        {
            if (GameManager.Instance.coin < 8)
                return;
            GameManager.Instance.coin -= 8;
            GameManager.Instance.AddLane();
            laneCovers[laneCounts].GetComponent<Animator>().Play("LaneCover_EnableLane", 0, 0f);
            lane_cost.text = "16 fc";
        }
        else if (laneCounts == 4)
        {
            if (GameManager.Instance.coin < 16)
                return;
            GameManager.Instance.coin -= 16;
            GameManager.Instance.AddLane();
            laneCovers[laneCounts].GetComponent<Animator>().Play("LaneCover_EnableLane", 0, 0f);
            lane_cost.text = "";
        }
        UpdateCoins();

    }

    public void BuyHeroCard(GameObject cardObj)
    {
        Cost cost = cardObj.GetComponent<Cost>();

        if (GameManager.Instance.coin < cost.cost)
            return;

        GameManager.Instance.coin -= cost.cost;

        cardObj.GetComponent<Button>().interactable = false;
        cardObj.GetComponent<Animator>().Play(cost.animationOnBuy, 0, 0f);

        shopDeck.AddCard(cardObj.GetComponent<FieldCard>().card);
        GameManager.Instance.deck.AddCard(cardObj.GetComponent<FieldCard>().card);

        UpdateCoins();
    }

    public void UpdateCoins()
    {
        fc_text.text = GameManager.Instance.coin.ToString();
    }

    public void DisplayLanes()
    {
        int laneCounts = GameManager.Instance.lanePanels.Count;
        foreach (GameObject lane in laneCovers)
        {
            lane.GetComponent<Animator>().Play("Lane_idle", 0, 0f);
        }
        for (int i = 0; i < laneCounts; i++)
        {
            laneCovers[i].GetComponent<Animator>().Play("LaneCover_EnableLane", 0, 0f);
        }
    }
}
