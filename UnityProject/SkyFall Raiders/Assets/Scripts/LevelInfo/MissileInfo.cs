using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Missile")]
public class MissileInfo : ScriptableObject
{
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private float blinkPeriod;
    public float BlinkPeriod => blinkPeriod;
    [SerializeField] private float duration;
    public float Duration => duration;
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
}