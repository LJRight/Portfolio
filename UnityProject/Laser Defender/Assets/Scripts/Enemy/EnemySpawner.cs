// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float timeBetwwenWaves = 0f;
    [SerializeField] bool isLooping = true;
    WaveConfigSO currentWave;
    public WaveConfigSO CurrentWave { get { return currentWave; } }
    [SerializeField] float speedToTargetPosition = 2.0f;
    List<Quaternion> FixedSpawnQuaternion;
    List<int> freePosition;
    const float SPRITE_FACING_OFFSET = -90f;
    public IEnumerator SpawnEnemyWaves(List<WaveConfigSO> waveConfigs)
    {
        do
        {
            int randomWaveIdx = Random.Range(0, waveConfigs.Count);
            currentWave = waveConfigs[randomWaveIdx];
            GameObject prefab = currentWave.Prefab;
            Vector3 startPos = currentWave.GetStartingWayPoint().position;
            List<Transform> wayPoints = currentWave.GetWayPoints();
            float moveSpeed = currentWave.MoveSpeed;
            for (int i = 0; i < currentWave.EnemyCount; i++)
            {
                GameObject instance = Instantiate(prefab, startPos,
                                                Quaternion.identity, transform);
                instance.GetComponent<PathFinder>().Init(wayPoints, moveSpeed);
                // instance.GetComponent<Health>().Init()
                yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
            }
            yield return new WaitForSeconds(timeBetwwenWaves);

        } while (isLooping);
    }
    public IEnumerator SpawnEnemy(float spawnInterval, Enemy enemy)
    {
        // 고정 스폰 위치를 가지는 적의 경우
        if (enemy.HasFixedSpawnPosition)
            InitQuaternion(enemy.SpawnPositions);

        while (true)
        {
            // 고정 스폰 위치를 가지는 적의 경우
            if (enemy.HasFixedSpawnPosition)
            {
                // 고정 스폰 위치가 전부 사용 중이라면, 기존 리스폰 시간의 절반만큼 대기
                if (freePosition.Count == 0)
                {
                    yield return new WaitForSeconds(spawnInterval / 2);
                    continue;
                }
                // 무작위 스폰위치 지정 후 적 생성
                int randomIdx = Random.Range(0, freePosition.Count);
                int positionIdx = freePosition[randomIdx];
                freePosition.RemoveAt(randomIdx);
                GameObject instance = Instantiate(enemy.Prefab, enemy.SpawnPositions[positionIdx].StartPosition, FixedSpawnQuaternion[positionIdx]);
                instance.GetComponent<Sniper>().Init(enemy.SpawnPositions[positionIdx].TargetPosition, positionIdx, enemy.Speed);
                instance.GetComponent<Health>().Init(enemy.HP, enemy.Point, enemy.Coin);
            }
            else
            {
                Vector2 randomSpawnPos = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0.1f, .9f), 1.1f));
                Vector2 randomTargetPos = new Vector2(randomSpawnPos.x, Camera.main.ViewportToWorldPoint(new Vector2(0, -0.1f)).y);
                GameObject instance = Instantiate(enemy.Prefab, randomSpawnPos, Quaternion.identity);
                Debug.Log(instance.transform.position);
                instance.GetComponent<LinearMove>()?.Init(randomTargetPos, enemy.Speed);
                instance.GetComponent<Health>().Init(enemy.HP, enemy.Point, enemy.Coin);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 비어있는 고정 스폰 위치를 추가하는 함수
    public void PositionClear(int idx)
    {
        if (!freePosition.Contains(idx))
            freePosition.Add(idx);
    }

    // 적의 Transform 초기 회전값을 캐싱하기 위한 함수 (스프라이트의 정면은 아래방향(-90도))
    private void InitQuaternion(List<SpawnPosition> SpawnPositions)
    {
        int n = SpawnPositions.Count;
        FixedSpawnQuaternion = new List<Quaternion>(n);
        freePosition = Enumerable.Range(0, n).ToList();
        foreach (SpawnPosition sp in SpawnPositions)
        {
            Vector2 dir = sp.StartPosition - sp.TargetPosition;
            FixedSpawnQuaternion.Add(Quaternion.Euler(0f, 0f, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + SPRITE_FACING_OFFSET));
        }
    }
}
