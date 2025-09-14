using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    [SerializeField] List<Shooter> LG, RG, Sniper;
    [SerializeField] Shooter Cannon;
    [SerializeField] float mvSpd;

    [Header("GatlingGun Pattern")]
    [SerializeField] float rate;
    [SerializeField] private int MinBulletCount, MaxBulletCount;
    [SerializeField] float spd;

    [Header("Cannon Sweep Pattern")]
    [SerializeField] float duration, rate_, stAg;
    Rigidbody2D myRb;
    enum PatternKind { P1_Left, P1_Right, P1_Both, P2_Sweep }
    [SerializeField] float minGap, maxGap;
    private Coroutine routine;
    void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
    }
    public void SpawnStart(Vector2 targetPos)
    {
        StartCoroutine(SpawnMove(targetPos));
    }
    private IEnumerator SpawnMove(Vector2 targetPos)
    {
        while (Vector2.Distance(transform.position, targetPos) >= Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, mvSpd * Time.deltaTime);
            yield return null;
        }
        Move();
    }
    private void Move()
    {
        myRb.linearVelocity = Vector2.right * mvSpd;
        routine = StartCoroutine(PatternLoop());
    }
    public void StopPattern()
    {
        if (routine != null)
            StopCoroutine(routine);
    }
    private IEnumerator PatternLoop()
    {

        var kinds = new PatternKind[] { PatternKind.P1_Left, PatternKind.P1_Right, PatternKind.P1_Both, PatternKind.P2_Sweep };

        while (true)
        {
            var pick = kinds[Random.Range(0, kinds.Length)];

            switch (pick)
            {
                case PatternKind.P1_Left:
                    yield return StartCoroutine(AttackPattern_1(LG));
                    break;

                case PatternKind.P1_Right:
                    yield return StartCoroutine(AttackPattern_1(RG));
                    break;

                case PatternKind.P1_Both:
                    yield return StartCoroutine(AttackPattern_1_1());
                    break;

                case PatternKind.P2_Sweep:
                    yield return StartCoroutine(AttackPattern_2(Cannon));
                    break;
            }
            // 다음 패턴까지 텀
            yield return new WaitForSeconds(Random.Range(minGap, maxGap));
        }
    }
    private IEnumerator AttackPattern_1(List<Shooter> target)
    {
        foreach (Shooter st in target)
        {
            st.SetMode(Shooter.FireMode.Straight);
            st.SetProjectileSpeed(spd);
        }
        int cnt = Random.Range(MinBulletCount, MaxBulletCount);
        int bulletCount = 0;
        while (bulletCount < cnt)
        {
            bulletCount++;
            foreach (Shooter st in target)
            {
                st.SpawnBullet(st.transform.position, st.GetFireDirection(st.Mode), Quaternion.identity);
            }
            yield return new WaitForSeconds(rate);
        }
    }
    private IEnumerator AttackPattern_1_1()
    {
        StartCoroutine(AttackPattern_1(LG));
        StartCoroutine(AttackPattern_1(RG));
        yield return null;
    }

    private IEnumerator AttackPattern_2(Shooter st)
    {
        st.SetProjectileSpeed(3.5f);
        float LstAg = Random.Range(0, 2) == 0 ? stAg : -stAg;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentAngle = Mathf.Lerp(LstAg, -LstAg, t);
            Vector3 dir = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.down;
            st.SpawnBullet(st.transform.position, dir, Quaternion.identity);
            yield return new WaitForSeconds(rate_);
            elapsed += rate_;
        }
    }
}

