using UnityEngine;

[CreateAssetMenu(menuName = "Level/Events/SpawnEnemy")]
public class SpawnEnemyEvent : TimelineEvent
{
    [SerializeField] private EnemyInfo enemy;
    public override void Execute(GameController gc)
    {
        gc.SpawnEnemy(SpawnInterval, enemy);
    }
}