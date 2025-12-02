using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> laneCovers = new List<GameObject>();

    public void Start()
    {
        EnableShop();
    }

    public void BuyLane()
    {

    }

    public void EnableShop()
    {
        DisplayLanes();
    }

    public void DisplayLanes()
    {
        Debug.Log("Hello");
        int laneCounts = GameManager.Instance.lanePanels.Count;
        foreach (GameObject lane in laneCovers)
        {
            lane.GetComponent<Animator>().Play("Lane_idle", 0, 0f);
        }
        for (int i = 0; i < laneCounts; i++)
        {
            Debug.Log(i);
            laneCovers[i].GetComponent<Animator>().Play("LaneCover_EnableLane", 0, 0f);
        }
    }
}
