using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Info", fileName = "New Level Info")]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] private List<TimelineEvent> timeline;
    [SerializeField] List<WaveConfigSO> wave_Infos;
    [SerializeField] private EnemyInfo basicEnemyInfo;
    public EnemyInfo BasicEnemy => basicEnemyInfo;
    public List<WaveConfigSO> Waves => wave_Infos;
    [SerializeField] private float difficultyIncreaseInterval;
    public float DifficultyIncreaseInterval => difficultyIncreaseInterval;
    [SerializeField] private float difficultyMultiplier;
    public float DifficultyMultiplier => difficultyMultiplier;
    [SerializeField] private float minTimeBetweenWave, maxTimeBetweenWave;
    public List<TimelineEvent> GetTimeLine()
    {
        timeline.Sort();
        return timeline;
    }
    public float GetRandomWaveTime()
    {
        return Random.Range(minTimeBetweenWave, maxTimeBetweenWave);
    }
}