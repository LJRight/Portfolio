using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class binary_right_UIManager : MonoBehaviour
{
    public static binary_right_UIManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI coinText;
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
    public void FindTMPGUI()
    {
        coinText = GameObject.Find("coin Text").GetComponent<TextMeshProUGUI>();
    }
    public void SetScoreText(int cur, int lvl)
    {
        coinText.text = cur + " / " + lvl;
    }
}
