using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<GameObject> backgrounds = new List<GameObject>();

    public void Start()
    {
        ChangeBackground(0);
    }

    public void ChangeBackground(int newIndex)
    {
        int i = 0;
        foreach (GameObject bg in backgrounds)
        {
            bg.SetActive(false);
            if (i == newIndex)
                bg.SetActive(true);
            i++;
        }

    }

}
