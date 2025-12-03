using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;


public class ScoreSystem : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public int comboCounter;
    public bool comboFlag;
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
                // Group cards by Rank and count how many of each Rank we have
                var rankGroups = lane.cards
                    .GroupBy(c => c.rank)
                    .Select(g => new { rank = g.Key, Count = g.Count() })
                    .ToList();

                bool hasPair = rankGroups.Any(g => g.Count == 2);
                bool hasThreeOfKind = rankGroups.Any(g => g.Count == 3);
                bool hasFourOfKind = rankGroups.Any(g => g.Count == 4);

                if (hasFourOfKind)
                {
                    AddPoints(Score.Four_of_a_kind);
                    lane.PopUpScore(Score.Four_of_a_kind);
                    lane.ClearLane();
                    return;
                }

                if (hasThreeOfKind)
                {
                    AddPoints(Score.Three_of_a_kind);
                    lane.PopUpScore(Score.Three_of_a_kind);
                    lane.ClearLane();
                    return;
                }

                if (lane.cards.Count() >= 5)
                {
                    AddPoints(Score.Very_Large_21);
                    lane.PopUpScore(Score.Very_Large_21);
                    lane.ClearLane();
                    return;
                }

                if (lane.cards.Count() >= 4)
                {
                    AddPoints(Score.Large_21);
                    lane.PopUpScore(Score.Large_21);
                    lane.ClearLane();
                    return;
                }

                if (hasPair)
                {
                    AddPoints(Score.Pair);
                    lane.PopUpScore(Score.Pair);
                    lane.ClearLane();
                    return;
                }

                if (lane.cards.Count() == 2)
                {
                    AddPoints(Score.BlackJack);
                    lane.PopUpScore(Score.BlackJack);
                    lane.ClearLane();
                    return;
                }

                if (lane.cards.Count() >= 5)
                {
                    AddPoints(Score.Normal_21);
                    lane.PopUpScore(Score.Normal_21);
                    lane.ClearLane();
                    return;
                }

                AddPoints(Score.Normal_21);
                lane.PopUpScore(Score.Normal_21);
                lane.ClearLane();
                return;
            }

            //Fail catch
            else if (lane.score > 21)
            {
                GameManager.Instance.RestartGame();
            }
        }
        comboFlag = false;
    }

    public void RestartScore()
    {
        GameManager.Instance.points = 0;
        scoreText.text = GameManager.Instance.points.ToString();
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
        finalScoreText.text = "You Won!\nFinal Score: " + GameManager.Instance.points.ToString();
    }
}
