// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] bool isLooping = true;
    WaveConfigSO currentWave;
    public WaveConfigSO CurrentWave { get { return currentWave; } }
    List<Quaternion> FixedSpawnQuaternion;
    List<int> freePosition;
    const float SPRITE_FACING_OFFSET = -90f;
    // 기본적 생성 루틴 
    public IEnumerator SpawnEnemyWaves(List<WaveConfigSO> waveConfigs, LevelInfoSO LevelInfo)
    {
        int waveCounter = 0;
        do
        {
            if (waveCounter == waveConfigs.Count)
            {
                waveCounter = 0;
                waveConfigs.Shuffle();
            }
            currentWave = waveConfigs[waveCounter++];
            GameObject prefab = currentWave.Prefab;
            Vector3 startPos = currentWave.GetStartingWayPoint().position;
            List<Transform> wayPoints = currentWave.GetWayPoints();
            float moveSpeed = currentWave.MoveSpeed;
            for (int i = 0; i < currentWave.EnemyCount; i++)
            {
                GameObject instance = Instantiate(prefab, startPos,
                                                Quaternion.identity, transform);
                instance.GetComponent<PathFinder>().Init(wayPoints, moveSpeed);
                instance.GetComponent<Health>().Init(LevelInfo.BasicEnemy.HP * Mathf.Floor(GameController.Instance.DifficultyLevel),
                                                     LevelInfo.BasicEnemy.Point,
                                                     LevelInfo.BasicEnemy.Coin);
                instance.GetComponent<Shooter>().Init(true,
                                                      LevelInfo.BasicEnemy.HasRandomFiringRate,
                                                      0,
                                                      LevelInfo.BasicEnemy.MinFiringRate,
                                                      LevelInfo.BasicEnemy.MaxFiringRate);
                yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
            }
            yield return new WaitForSeconds(LevelInfo.GetRandomWaveTime());
        } while (isLooping);
    }
    // 스나이퍼, 중간 보스 생성 루틴
    public IEnumerator SpawnEnemy(float spawnInterval, EnemyInfo enemy)
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
                GameObject instance = Instantiate(enemy.Prefab,
                                                  enemy.SpawnPositions[positionIdx].StartPosition,
                                                  FixedSpawnQuaternion[positionIdx]);

                instance.GetComponent<Sniper>().Init(enemy.SpawnPositions[positionIdx].TargetPosition,
                                                     positionIdx,
                                                     enemy.Speed);
                instance.GetComponent<Health>().Init(enemy.HP * GameController.Instance.DifficultyLevel,
                                                     enemy.Point,
                                                     enemy.Coin);
            }
            else
            {
                Vector2 randomSpawnPos = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0.1f, .9f), 1.1f));
                Vector2 randomTargetPos = new Vector2(randomSpawnPos.x, Camera.main.ViewportToWorldPoint(new Vector2(0, -0.1f)).y);
                GameObject instance = Instantiate(enemy.Prefab, randomSpawnPos, Quaternion.identity);

                instance.GetComponent<LinearMove>()?.Init(randomTargetPos, enemy.Speed);
                instance.GetComponent<Health>().Init(enemy.HP * GameController.Instance.DifficultyLevel,
                                                     enemy.Point,
                                                     enemy.Coin);
                foreach (Shooter st in instance.GetComponentsInChildren<Shooter>())
                    st.Init(true, enemy.HasRandomFiringRate, enemy.FiringRate, 0, 0);

            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBoss()
    {
        
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
