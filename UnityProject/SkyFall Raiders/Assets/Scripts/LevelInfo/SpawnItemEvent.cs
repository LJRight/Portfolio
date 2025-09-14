using UnityEngine;

[CreateAssetMenu(menuName = "Level/Events/SpawnItem")]
public class SpawnItemEvent : TimelineEvent
{
    [SerializeField] private GameObject item;
    private static float offset = .05f;
    public override void Execute(GameController gc)
    {
        gc.StartSpawnItem(SpawnInterval, item);
    }
    public static Vector2 GetRandomSpawnPosistion()
    {
        return Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(0f + offset, 1f - offset), 1f + offset));
    }
}