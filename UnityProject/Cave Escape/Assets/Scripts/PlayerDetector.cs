using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private GameObject alert; // 느낌표 오브젝트
    [SerializeField] private Vector2 offset = new Vector2(0, 5.9f); // 머리 위로 위치 조정
    [SerializeField] private float attackBoxYoffset = 2.5f, attackBoxXoffset = 3.0f;
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private float findRange = 8.0f, exitRange = 10.0f;
    [SerializeField] private LayerMask playerLayer; // 플레이어 감지 레이어
    [SerializeField] private Vector2 boxSize = new Vector2(2.8f, 4.8f);
    [SerializeField] private EnemyController enemy;
    [SerializeField] private GameObject target;
    private Transform pos;
    public bool inRange;
    public bool InRange { get { return inRange; } }
    public bool inAttackRange;
    public bool InAttackRange { get { return inAttackRange; } }
    public float playerDirection;
    public float PlayerDirection { get { return playerDirection; } }
    private bool found = false;
    private float attackDetectDelay = 0.5f;
    private bool canDetectAttackRange = true;
    private void Awake()
    {
        alert.SetActive(false); // 시작 시 비활성화
        enemy = GetComponent<EnemyController>();
        pos = transform; // 몬스터의 Transform
        inRange = false;
        inAttackRange = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, exitRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, findRange);
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(enemy.Direction * attackBoxXoffset, attackBoxYoffset);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    private void Update()
    {
        // 느낌표를 몬스터 머리 위에 위치시킴
        alert.transform.position = (Vector2)pos.position + offset;
        DetectPlayer();
        DetectAttack();
        GetPlayerDirection();
    }
    void DetectPlayer()
    {
        float distanceToPlayer = GetPlayerDistance();
        if (distanceToPlayer <= findRange) // 진입 범위 조건
        {
            Collider2D player = Physics2D.OverlapCircle(pos.position, findRange, playerLayer);
            if (player != null)
            {
                inRange = true;
                if (!found) Showalert();
                return;
            }
        }
        else if (distanceToPlayer > exitRange) // 탈출 범위 조건
        {
            inRange = false;
            found = false;
            alert.SetActive(false);
        }
    }
    float GetPlayerDistance()
    {
        Collider2D player = Physics2D.OverlapCircle(pos.position, exitRange, playerLayer);
        if (player != null)
        {
            return Vector2.Distance(transform.position, player.transform.position);
        }
        return Mathf.Infinity; // 플레이어가 없으면 무한대 반환
    }
    void GetPlayerDirection()
    {
        playerDirection = target.transform.position.x < transform.position.x ? -1 : 1;
    }
    void DetectAttack()
    {
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(enemy.Direction * attackBoxXoffset, attackBoxYoffset);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);
        if (hit != null)
        {
            if (canDetectAttackRange)
            {
                inAttackRange = true;
                StartCoroutine(WaitNextInAttackRangeDetect());
            }
        }
        else
            inAttackRange = false;
    }
    public void Showalert()
    {
        alert.SetActive(true); // 느낌표 활성화
        found = true;
        StartCoroutine(HidealertAfterDelay()); // 일정 시간 뒤 비활성화
    }
    private IEnumerator HidealertAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        alert.SetActive(false); // 느낌표 비활성화
    }
    private IEnumerator WaitNextInAttackRangeDetect()
    {
        canDetectAttackRange = false;
        yield return new WaitForSeconds(attackDetectDelay);
        canDetectAttackRange = true;
    }
}
