using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Wave Config", fileName = "New Wave Config")]
public class WaveConfigSO : ScriptableObject
{
    [SerializeField] private GameObject enemyPrefab;
    public GameObject Prefab => enemyPrefab;
    [SerializeField] private int enemyCount;
    public int EnemyCount => enemyCount;
    [SerializeField] Transform pathPrefab;
    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;
    [SerializeField] float timeBetweenEnemySpawns = 1f;
    [SerializeField] float spawnTimeVariance = 0f;
    [SerializeField] float minimumSpawnTime = 0.2f;
    public enum RandomStartPosDirect { None, X, Y };
    [SerializeField] float minBound, maxBound;
    [SerializeField] RandomStartPosDirect random;
    public Transform GetStartingWayPoint()
    {
        return (random != RandomStartPosDirect.None) ? InitRandomPos() : pathPrefab.GetChild(0);
    }
    Transform InitRandomPos()
    {
        Vector2 pos = pathPrefab.transform.position;
        if (random == RandomStartPosDirect.X)
            pos.x = Random.Range(minBound, maxBound);
        else if (random == RandomStartPosDirect.Y)
            pos.y = Random.Range(minBound, maxBound);
        pathPrefab.transform.position = pos;
        return pathPrefab.GetChild(0);
    }
    public List<Transform> GetWayPoints()
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in pathPrefab)
            result.Add(child);
        return result;
    }
    public GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }
    public float GetRandomSpawnTime()
    {
        float spawnTime = Random.Range(timeBetweenEnemySpawns - spawnTimeVariance, timeBetweenEnemySpawns + spawnTimeVariance);
        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}
