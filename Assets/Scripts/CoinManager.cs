using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CoinManager : MonoBehaviour
{
    public TMP_Text pointsText;
    public TMP_Text coinsText;

    public void DisplayPoints()
    {
        pointsText.text = GameManager.Instance.points.ToString();
        coinsText.text = GameManager.Instance.coin.ToString();

        
    }

    public void giveCoins()
    {
        // Animate A
        DOTween.To(() => GameManager.Instance.points, x =>
        {
            GameManager.Instance.points = x;
            pointsText.text = GameManager.Instance.points.ToString();
        }, 0, 1);

        // Animate B
        DOTween.To(() => GameManager.Instance.coin, x =>
        {
            GameManager.Instance.coin = x;
            coinsText.text = GameManager.Instance.coin.ToString();
        }, (GameManager.Instance.coin + GameManager.Instance.points)/100, 1);
    }
}
