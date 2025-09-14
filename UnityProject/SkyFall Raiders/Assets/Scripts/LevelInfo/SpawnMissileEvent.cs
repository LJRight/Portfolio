using UnityEngine;

[CreateAssetMenu(menuName = "Level/Events/SpawnMissile")]
public class SpawnMissileEvent : TimelineEvent
{
    [SerializeField] private MissileInfo missileInfo;
    private static float offset = .05f;
    public override void Execute(GameController gc)
    {
        gc.SpawnMissile(SpawnInterval, missileInfo);
    }
    public static Vector2 GetRandomSpawnPosistion()
    {
        return Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0f + offset, 1f - offset), 1f + offset));
    }
}