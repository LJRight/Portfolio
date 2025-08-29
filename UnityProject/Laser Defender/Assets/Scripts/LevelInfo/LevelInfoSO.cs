using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Info", fileName = "New Level Info")]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] private List<TimelineEvent> timeline;
    [SerializeField] List<WaveConfigSO> wave_Infos;
    public List<WaveConfigSO> Waves => wave_Infos;
    public List<TimelineEvent> GetTimeLine()
    {
        timeline.Sort();
        return timeline;
    }
}