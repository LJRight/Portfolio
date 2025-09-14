using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    List<Coroutine> MissileSpawnCoroutine;
    private int cursor = 0;
    private float gameTime = 0f;
    private float difficultyLevelTimer = 0f;
    private float currentDifficultyLevel = 1.0f;
    public float DifficultyLevel => currentDifficultyLevel;
    private bool inLevelScene;
    bool IsGameClear = false;
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
            IsGameClear = false;
            inLevelScene = true;
            cursor = 0;
            ResetGameTime();
            ResetLevel();
            enemySpawner = FindFirstObjectByType<EnemySpawner>();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentLevelInfo = level_Infos[sceneIndex - 1];
            timeline = currentLevelInfo.GetTimeLine();
            SpawnCoroutineInit();
            GetGameTime();
            UIManager.Instance.OnLevelSceneLoaded();
            if (enemySpawner != null)
                enemySpawner.StartSpawnEnemyWaves(currentLevelInfo.Waves, currentLevelInfo);
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
                    UIManager.Instance.OnGameOverSceneLoaded(IsGameClear);
                    break;
                default:
                    break;
            }
        }
    }
    private void SpawnCoroutineInit()
    {
        ItemSpawnCoroutine = new List<Coroutine>();
    }
    void Update()
    {
        if (timeline != null && inLevelScene)
            UpdateTime();
    }
    public void SpawnEnemy(float spawnInterval, EnemyInfo enemy)
    {
        if (enemySpawner != null)
            enemySpawner.StartSpawnEnemy(spawnInterval, enemy);
    }
    public void SpawnBoss(EnemyInfo boss)
    {
        GameObject instance = Instantiate(boss.Prefab, Camera.main.ViewportToWorldPoint(boss.SpawnPositions[0].StartPosition), Quaternion.identity);
        instance.GetComponent<BossPattern>().SpawnStart(Camera.main.ViewportToWorldPoint(boss.SpawnPositions[0].TargetPosition));
        if (enemySpawner != null)
            enemySpawner.StopEnemySpawnRoutine();
        // StopSpawnRoutine(ItemSpawnCoroutine);
    }
    public void StartSpawnItem(float spawnInterval, GameObject item)
    {
        ItemSpawnCoroutine.Add(StartCoroutine(SpawnItemCoroutine(spawnInterval, item)));
    }
    public void SpawnMissile(float spawnInterval, MissileInfo info)
    {
        if (enemySpawner != null)
            enemySpawner.StartSpawnMissile(spawnInterval, info);
    }
    private IEnumerator SpawnItemCoroutine(float spawnInterval, GameObject item)
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
            UpdateDifficulty();
        UIManager.Instance.SetTimeText(elapsedTime);
        while (cursor < timeline.Count && timeline[cursor].Time <= elapsedTime)
        {
            timeline[cursor].Execute(this);
            cursor++;
        }
    }
    public void GameOver()
    {
        ResetGameTime();
        UIManager.Instance.SFM.SlideToScene(SceneManager.sceneCountInBuildSettings - 1);
    }
    private void StopCoroutines(List<Coroutine> routines)
    {
        foreach (Coroutine routine in routines)
            StopCoroutine(routine);
    }
    private void StopItemSpawnRoutine()
    {
        if (ItemSpawnCoroutine != null)
            StopCoroutines(ItemSpawnCoroutine);
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
        StopItemSpawnRoutine();
        currentDifficultyLevel = 1.0f;
        difficultyLevelTimer = 0f;
    }
    private void UpdateDifficulty()
    {
        difficultyLevelTimer = 0;
        currentDifficultyLevel *= currentLevelInfo.DifficultyMultiplier;
        Debug.Log("Difficulty Level Updated! (current level : " + currentDifficultyLevel + ")");
        UIManager.Instance.SetPlayerDPSText();
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
    private void GetGameTime()
    {
        SpawnBossEvent bossTimeEvent = timeline.OfType<SpawnBossEvent>().FirstOrDefault();
        if (bossTimeEvent == null)
        {
            Debug.Log("Can not Find SpawnBossEvent in TimeLine List!");
            return;
        }
        gameTime = bossTimeEvent.Time;
    }
    public float GetProgress()
    {
        return elapsedTime / gameTime;
    }
    public float GetEnemyAttackSpeedRatioByTime()
    {
        return Mathf.Lerp(1.0f, currentLevelInfo.MaxEnemyAttackSpeedMultiplier, elapsedTime / gameTime);
    }
    public void ClearLevel()
    {
        IsGameClear = true;
        StopItemSpawnRoutine();
        UIManager.Instance.SFM.SlideToScene(SceneManager.sceneCountInBuildSettings - 1);
    }
}
