using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    float correctAnswer = 0;
    float questionsSeen = 0;
    float score = 100;
    public float Score { get { return score; } }
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = "Score : 100%";
    }
    void GetScore()
    {
        score = correctAnswer / questionsSeen * 100;
    }
    public void ChangeScore(bool isCorrect)
    {
        questionsSeen++;
        if (isCorrect)
            correctAnswer++;
        GetScore();
        scoreText.text = "Score : " + score.ToString("F2") + "%";
    }
}
