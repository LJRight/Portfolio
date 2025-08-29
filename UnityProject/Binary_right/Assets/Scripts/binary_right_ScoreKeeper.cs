using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class binary_right_ScoreKeeper : MonoBehaviour
{
    public static binary_right_ScoreKeeper Instance { get; private set; }
    [SerializeField] int levelScore, currentScore;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetScore(int score)
    {
        currentScore = 0;
        levelScore = score;
        binary_right_UIManager.Instance.SetScoreText(currentScore, levelScore);
    }
    public void AddScore()
    {
        currentScore++;
        binary_right_UIManager.Instance.SetScoreText(currentScore, levelScore);
        if (currentScore == levelScore)
            binary_right_GameManager.Instance.ClearLevel();
    }
    public void ResetScore(int levelScore)
    {
        this.levelScore = levelScore;
        currentScore = 0;
    }
}
