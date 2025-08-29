using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [Header("For AI")]
    [SerializeField] bool useAI = false;
    [SerializeField] bool towardPlayer = false;
    [SerializeField] bool isBurst = false;
    [SerializeField] bool isSpriteRotating = false;
    [SerializeField] bool isTwinCannon = false;
    [SerializeField] float burstCooldown = 1.0f;
    [SerializeField] int minBurstCnt = 3, maxBurstCnt = 6;
    [SerializeField] FireMode myMode;
    enum FireMode { Straight, Toward, TowardWithRotation }
    [Header("Bullet")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 14;
    [SerializeField] float firingRate = 0.5f;
    public float FiringRate { get => firingRate; set => firingRate = value; }
    public float Damage { set => damageMultiplier = value; }
    float damageMultiplier = 1.0f;
    Coroutine currentCoroutine;
    Transform playerTransform;

    // Find the Transform of the non-player (enemy) player
    void Awake()
    {
        if (useAI)
            playerTransform = FindFirstObjectByType<PlayerMovement>()?.transform;
    }
    // Terminates the running Firing coroutine
    public void StopFiring()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
    // Firing Initiation Function (Enemy Exclusive)
    public void StartFiring()
    {
        StartCoroutine(FireRoutine(myMode));
    }
    // Firing Initiation Function (Player Exclusive)
    public void StartFiring(bool audioWorking)
    {
        currentCoroutine = StartCoroutine(FireContinuously(audioWorking));
    }
    IEnumerator FireContinuously(bool audioWorking)
    {
        while (true)
        {
            SpawnBullet(transform.position, transform.up, Quaternion.identity, audioWorking);
            yield return new WaitForSeconds(firingRate);
        }
    }
    IEnumerator FireRoutine(FireMode mode)
    {
        while (true)
        {
            // 씬에서 Player 의 transform 을 찾지 못한 경우 잠시 대기
            if (!IsPlayerValid())
            {
                playerTransform = FindFirstObjectByType<PlayerMovement>()?.transform;
                yield return null;
                continue;
            }
            Vector3 fireDirection = GetFireDirection(mode);
            Quaternion rotation = (mode == FireMode.TowardWithRotation)
                ? Quaternion.AngleAxis(Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg - 90f, Vector3.forward)
                : Quaternion.identity;

            int count = isBurst ? Random.Range(minBurstCnt, maxBurstCnt) : 1;
            for (int i = 0; i < count; i++)
            {
                SpawnBullet(transform.position, fireDirection, rotation);
                yield return new WaitForSeconds(firingRate);
            }
            if (isBurst)
                yield return new WaitForSeconds(burstCooldown);
        }
    }
    void SpawnBullet(Vector3 position, Vector3 direction, Quaternion rotation, bool playAudio = false)
    {
        GameObject instance = Instantiate(projectilePrefab, position, rotation);
        instance.GetComponent<DamageDealer>().SetDamage(damageMultiplier);
        if (playAudio)
            AudioPlayer.Instance.PlayShootingClip();
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction.normalized * projectileSpeed;
    }
    bool IsPlayerValid()
    {
        return playerTransform != null && playerTransform.gameObject.GetComponent<Collider2D>()?.enabled == true;
    }
    Vector3 GetFireDirection(FireMode mode)
    {
        return (mode == FireMode.Straight) ? -transform.up : (playerTransform.position - (isTwinCannon ? transform.parent.position : transform.position)).normalized;
    }
    public void SetBullet(GameObject bullet)
    {
        projectilePrefab = bullet;
    }
}
