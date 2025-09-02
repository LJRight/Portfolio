using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [Header("Game Setting")]
    public float currentDifficultyLevel = 1.0f;
    public bool isVibrate = true;
    private float elapsedTime;

    [Header("Level Information")]
    [SerializeField] List<LevelInfoSO> level_Infos;
    LevelInfoSO currentLevelInfo;
    EnemySpawner enemySpawner;
    Coroutine currentCoroutine;
    List<TimelineEvent> timeline;
    List<Coroutine> EnemySpawnCoroutine;
    private float difficultyLevelTimer = 0f;
    private int cursor = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneName.StartsWith("Level"))
        {
            cursor = 0;
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentLevelInfo = level_Infos[sceneIndex - 1];
            timeline = currentLevelInfo.GetTimeLine();
            EnemySpawnCoroutine = new List<Coroutine>();
            UIManager.Instance.OnLevelSceneLoaded();
            EnemySpawnCoroutine.Add(StartCoroutine(enemySpawner.SpawnEnemyWaves(currentLevelInfo.Waves)));
        }
        else
        {
            switch (sceneName)
            {
                case "Main Menu":
                    UIManager.Instance.OnMainMenuSceneLoaded();
                    break;
                case "Game Over":
                    UIManager.Instance.OnGameOverSceneLoaded();
                    break;
                default:
                    break;
            }
        }
    }
    void Update()
    {
        if (timeline != null)
            UpdateTime();
    }
    public void SpawnEnemy(float spawnInterval, Enemy enemy)
    {
        EnemySpawnCoroutine.Add(StartCoroutine(enemySpawner.SpawnEnemy(spawnInterval, enemy)));
    }
    // public void SpawnItem(float spawnInterval, Item item)
    // {

    // }
    public void StopGameTime()
    {
        Time.timeScale = 0f;
    }
    public void PlayGameTime()
    {
        Time.timeScale = 1f;
    }
    void UpdateTime()
    {
        elapsedTime += Time.deltaTime;
        difficultyLevelTimer = elapsedTime;
        UIManager.Instance.SetTimeText(elapsedTime);
        while (cursor < timeline.Count && timeline[cursor].Time <= elapsedTime)
        {
            timeline[cursor].Execute(this);
            cursor++;
        }
    }
    public void GameOver()
    {
        foreach (Coroutine coroutine in EnemySpawnCoroutine)
            StopCoroutine(coroutine);
    }
    public void ResetGameTime()
    {
        elapsedTime = 0f;
        UIManager.Instance.SetTimeText(elapsedTime);
    }
    public void ChangeVibrateMode(bool option)
    {
        isVibrate = option;
    }

    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.35f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
