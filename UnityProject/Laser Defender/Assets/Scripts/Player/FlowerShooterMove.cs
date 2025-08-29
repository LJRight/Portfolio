using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowerMove : MonoBehaviour
{
    [SerializeField] private float followSpeed = 3.5f;
    [SerializeField] private float noiseAmplitude = 0.2f;
    [SerializeField] private float noiseFrequency = 1.0f;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private float noiseOffsetX;
    private float noiseOffsetY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 서로 다른 흔들림을 위해 랜덤 offset 부여
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
    }
    public void SetTarget(Vector2 pos)
    {
        targetPos = pos;
    }
    void FixedUpdate()
    {
        // Perlin 노이즈 기반 흔들림 계산
        float time = Time.time;
        float noiseX = (Mathf.PerlinNoise(noiseOffsetX, time * noiseFrequency) - 0.5f) * 2f * noiseAmplitude;
        float noiseY = (Mathf.PerlinNoise(noiseOffsetY, time * noiseFrequency) - 0.5f) * 2f * noiseAmplitude;

        Vector2 noisyTarget = targetPos + new Vector2(noiseX, noiseY);

        // 부드럽게 따라가기
        Vector2 newPos = Vector2.Lerp(rb.position, noisyTarget, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
