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
            pointsText.text = x.ToString();
        }, 0, 1);

        // Animate B
        int currentValue = int.Parse(coinsText.text);
        int start = int.Parse(coinsText.text);
        int end = GameManager.Instance.coin + GameManager.Instance.points / 100;

        DOTween.To(() => start, x => {
            start = x;
            coinsText.text = x.ToString();
        }, end, 1f)
        .OnComplete(() => currentValue = end);


        GameManager.Instance.coin += GameManager.Instance.points / 100;
    }
}
