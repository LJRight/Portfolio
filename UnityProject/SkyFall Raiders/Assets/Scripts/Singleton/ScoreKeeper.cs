using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance { get; private set; }
    int score = 0;
    int powerUpScore = 0;
    // Base score for obtaining the first power-up item
    [SerializeField] private int powerUpCriterionBase = 100;
    // Score required to obtain the next power-up item
    [SerializeField] int powerUpCriterion = 125;
    int coin = 0;
    public int Score => score;
    public int Coin => coin;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void AddScore(int score)
    {
        this.score = Mathf.Clamp(this.score + score, 0, int.MaxValue);
        powerUpScore = Mathf.Clamp(powerUpScore + score, 0, int.MaxValue);
        UIManager.Instance.SetScoreText(this.score);
    }
    public void AddCoin(int coin)
    {
        this.coin = Mathf.Clamp(this.coin + coin, 0, int.MaxValue);
        UIManager.Instance.SetCoinText(this.coin);
    }
    public void ResetScore()
    {
        score = 0;
        coin = 0;
        powerUpCriterion = powerUpCriterionBase;
        powerUpScore = 0;
        UIManager.Instance.SetCoinText(coin);
        UIManager.Instance.SetScoreText(score);
    }
    public bool CheckPowerUpItemScore()
    {
        if (powerUpScore >= powerUpCriterion)
        {
            powerUpCriterion *= 2;
            powerUpScore = 0;
            return true;
        }
        else
            return false;
    }
}