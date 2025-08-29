using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private float hp;  // 체력
    public float HP => hp;
    [SerializeField] GameObject prefab; // 적 프리팹
    public GameObject Prefab => prefab;
    [SerializeField] private float speed;   // 이동속도
    public float Speed => speed;
    [SerializeField] private float firingRate;  // 발사 속도
    public float FiringRate => firingRate;
    [SerializeField] private float projectileSpeed; // 총알 속도
    public float ProjectileSpeed => projectileSpeed;
    [SerializeField] bool hasFixedSpawnPosition;    // 고정 스폰 위치를 가지는 지
    public bool HasFixedSpawnPosition => hasFixedSpawnPosition;
    [SerializeField] List<SpawnPosition> spawnPositions;    // 고정 스폰 위치에 대한 리스트
    public List<SpawnPosition> SpawnPositions => spawnPositions;
}

[Serializable]
public struct SpawnPosition
{
    [SerializeField] private Vector2 startPos, targetPos;
    public Vector2 StartPosition => startPos;
    public Vector2 TargetPosition => targetPos;
}