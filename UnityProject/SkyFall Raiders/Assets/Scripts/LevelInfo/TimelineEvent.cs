
using System;
using UnityEngine;
public abstract class TimelineEvent : ScriptableObject, IComparable<TimelineEvent>
{
    [SerializeField] private float time, spawnInterval;
    public float Time => time;
    public float SpawnInterval => spawnInterval;
    public abstract void Execute(GameController gc);
    public int CompareTo(TimelineEvent other) => Time.CompareTo(other.Time);
}