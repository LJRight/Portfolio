using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Player Info")]
    public int hp;
    public int maxHp;
    public bool isDead;

    [Header("# Game Info")]
    public int stage;
    public int currentPiece;
    public bool hasKey;
    public int talkId;
    public bool isCliff;

    public GameObject lightEnergyObject;
    public GameObject GameOverUI;
    public GameObject GameOverImage;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        hp = maxHp;
        stage = 0;
        currentPiece = 0;
    }

    void Update()
    {
        //if (lightEnergyObject == null)
        //{
        //    lightEnergyObject = GameObject.Find("BluelightEnergy");
        //}

        if (lightEnergyObject == null)
        {
            GameObject UIObject = GameObject.Find("UI");
            if (UIObject != null)
            {
                lightEnergyObject = UIObject.transform.Find("BluelightEnergy")?.gameObject;
            }
        }

        if (stage >= 2 && !lightEnergyObject.activeSelf)
            lightEnergyObject.SetActive(true);
    }

    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        if (stage == 0)
        {
            this.talkId = 100;
            UIManager.instance.Action();
        }
    }

    public void GameOverScene()
    {
        if (GameOverUI != null)
        {
            GameOverImage = GameOverUI.transform.Find("GameOverImage")?.gameObject;
            if (GameOverImage != null)
            {
                GameOverImage.SetActive(true);
            }
        }

        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
    }
}
