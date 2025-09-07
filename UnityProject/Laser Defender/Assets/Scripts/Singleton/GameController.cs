using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [Header("Game Setting")]
    public bool isVibrate = true;
    private float elapsedTime;

    [Header("Level Information")]
    [SerializeField] List<LevelInfoSO> level_Infos;
    private LevelInfoSO currentLevelInfo;
    EnemySpawner enemySpawner;
    List<TimelineEvent> timeline;
    List<Coroutine> EnemySpawnCoroutine;
    List<Coroutine> ItemSpawnCoroutine;
    private int cursor = 0;
    private float difficultyLevelTimer = 0f;
    private float currentDifficultyLevel = 1.0f;
    public float DifficultyLevel => currentDifficultyLevel;
    private bool inLevelScene;
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
        Application.targetFrameRate = 60;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneName.StartsWith("Level"))
        {
            inLevelScene = true;
            cursor = 0;
            ResetGameTime();
            ResetLevel();
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentLevelInfo = level_Infos[sceneIndex - 1];
            timeline = currentLevelInfo.GetTimeLine();
            EnemySpawnCoroutine = new List<Coroutine>();
            ItemSpawnCoroutine = new List<Coroutine>();
            UIManager.Instance.OnLevelSceneLoaded();
            if(enemySpawner != null)
                EnemySpawnCoroutine.Add(StartCoroutine(enemySpawner.SpawnEnemyWaves(currentLevelInfo.Waves, currentLevelInfo)));
        }
        else
        {
            inLevelScene = false;
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
        if (timeline != null && inLevelScene)
            UpdateTime();
    }
    public void SpawnEnemy(float spawnInterval, EnemyInfo enemy)
    {
        EnemySpawnCoroutine.Add(StartCoroutine(enemySpawner.SpawnEnemy(spawnInterval, enemy)));
    }
    public void StartSpawnItem(float spawnInterval, GameObject item)
    {
        ItemSpawnCoroutine.Add(StartCoroutine(SpawnItemCoroutine(spawnInterval, item)));
    }
    IEnumerator SpawnItemCoroutine(float spawnInterval, GameObject item)
    {
        var interval = new WaitForSeconds(spawnInterval);
        while (true)
        {
            Instantiate(item, SpawnItemEvent.GetRandomSpawnPosistion(), Quaternion.identity);
            yield return interval;
        }
    }
    public void StopGameTime()
    {
        UIManager.Instance.JoyStick.Lock();
        Time.timeScale = 0f;
    }
    public void PlayGameTime()
    {
        UIManager.Instance.JoyStick.Unlock();
        Time.timeScale = 1f;
    }
    void UpdateTime()
    {
        elapsedTime += Time.deltaTime;
        difficultyLevelTimer += Time.deltaTime;
        if (difficultyLevelTimer >= currentLevelInfo.DifficultyIncreaseInterval)
        {
            difficultyLevelTimer = 0;
            currentDifficultyLevel *= currentLevelInfo.DifficultyMultiplier;
            Debug.Log("Difficulty Level Updated! (current level : " + currentDifficultyLevel + ")");    
        }
        UIManager.Instance.SetTimeText(elapsedTime);
        while (cursor < timeline.Count && timeline[cursor].Time <= elapsedTime)
        {
            timeline[cursor].Execute(this);
            cursor++;
        }
    }
    public void GameOver()
    {
        StopSpawnRoutine();
        ResetGameTime();
        UIManager.Instance.SFM.SlideToScene(SceneManager.sceneCountInBuildSettings - 1);
    }
    private void StopSpawnRoutine()
    {
        foreach (Coroutine coroutine in EnemySpawnCoroutine)
            StopCoroutine(coroutine);
        foreach (Coroutine coroutine in ItemSpawnCoroutine)
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
    public void ResetLevel()
    {
        currentDifficultyLevel = 1.0f;
        difficultyLevelTimer = 0f;
    }
    private float UpdateDifficulty()
    {
        float playerRelativePower = FindAnyObjectByType<PlayerLevel>().GetPlayerPowerMultiplier();
        Debug.Log("Player Relative Power : " + playerRelativePower);
        float nextDifficultyLevel = currentDifficultyLevel * currentLevelInfo.DifficultyMultiplier;
        return playerRelativePower > nextDifficultyLevel ? playerRelativePower : nextDifficultyLevel;
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
