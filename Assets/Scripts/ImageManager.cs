using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Image image;


    public void FadeInRight()
    {
        image.GetComponent<Animator>().SetTrigger("FadeInRight");
    }
    public void FadeInLeft()
    {

    }

    public void Fade()
    {

    }

    public void ChangeImage(Sprite newImage)
    {
        image.sprite = newImage;
    }

}
