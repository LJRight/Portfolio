using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class binary_right_GameManager : MonoBehaviour
{
    public static binary_right_GameManager Instance { get; private set; }
    [SerializeField] List<int> levelCoinInfo;
    [SerializeField] int totalSceneCount = 3;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cursor.visible = false;
        binary_right_UIManager.Instance.FindTMPGUI();
        binary_right_ScoreKeeper.Instance.SetScore(levelCoinInfo[scene.buildIndex]);
    }
    public void ClearLevel()
    {
        int nextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneNum == totalSceneCount)
        {
            Application.Quit();
        }
        else
            SceneManager.LoadScene(nextSceneNum);
    }
}
