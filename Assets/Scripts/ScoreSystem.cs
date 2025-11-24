using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;


public class ScoreSystem : MonoBehaviour
{
    public TMP_Text scoreText;
    private int comboCounter;
    private bool comboFlag;
    public ParticleSystem sparkles;

    public void ScoreProccess()
    {
        if (GameManager.Instance.lanePanels.Count >= 5)
        {
            //Check if all lanes have the same score
            if (GameManager.Instance.lanePanels.All(c => c.score == GameManager.Instance.lanePanels[0].score) && GameManager.Instance.lanePanels[0].score != 0)
            {
                AddPoints(Score.Five_of_a_Kind);
                GameManager.Instance.ClearAllLanes();
                return;
            }

            //Check if all lanes are in sequence
            List<int> scores = GameManager.Instance.lanePanels.Select(c => c.score).ToList();

            int min = scores.Min();
            int max = scores.Max();

            if (scores.Distinct().Count() == scores.Count && (max - min + 1) == scores.Count && min != 0)
            {
                AddPoints(Score.Straight);
                GameManager.Instance.ClearAllLanes();
                return;
            }
        }


        //check each lane
        foreach (LanePanel lane in GameManager.Instance.lanePanels)
        {
            if (lane.score == 21)
            {
                AddPoints(Score.Normal_21);
                GameManager.Instance.discard.cards.AddRange(lane.GetAllCards());
                lane.ClearLane();
                return;
            }
            else if (lane.score > 21)
            {
                Debug.Log("You Blew up");
                GameManager.Instance.RestartGame();
            }
        }
        comboFlag = false;
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

        int targetPoints = (int)score * multiplyer + GameManager.Instance.points;

        DOTween.To(() => GameManager.Instance.points, x => {
            GameManager.Instance.points = x;
            scoreText.text = GameManager.Instance.points.ToString();
        }, targetPoints, 0.4f)
        .SetEase(Ease.OutQuad);
        scoreText.transform.DOPunchScale(
        new Vector3(0.3f, 0.3f, 0), 0.5f);

        scoreText.DOColor(Color.yellow, 0.15f)
        .OnComplete(() => scoreText.DOColor(Color.white, 0.2f));

        sparkles.Play();

        comboFlag = true;
    }
}
