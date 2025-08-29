using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    ScoreKeeper scoreKeeper;
    TextMeshProUGUI scoreText;
    void Start()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    public void ActiveCanvas()
    {
        scoreText.text = "Congratulations!\nYou scored " + scoreKeeper.Score + "%";
        gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
