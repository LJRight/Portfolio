using UnityEngine;

[CreateAssetMenu(menuName = "Level/Events/SpawnBoss")]
public class SpawnBossEvent : TimelineEvent
{
    [SerializeField] private EnemyInfo enemy;
    public override void Execute(GameController gc)
    {
        gc.SpawnBoss(enemy);
    }
}