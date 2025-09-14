using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Idle, Moving, Death, Chasing, Attacking, GetAway }
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float direction = 1.0f; // 몬스터 초기 방향 (오른쪽)
    public float Direction { get { return direction; } }
    private int defaultLayer; // 원래의 레이어
    [SerializeField] private float speed = 3f, minSpeed = 1.0f, maxSpeed = 5.0f, basicSpeed = 3.0f;
    public float Speed { get { return speed; } set { speed = value; } }
    public float MinSpeed { get { return minSpeed; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float BasicSpeed { get { return basicSpeed; } }
    [SerializeField] private int hp = 5;    // 체력
    public int HP { get { return hp; } set { hp -= value; } }
    [SerializeField] private float stunDuration = 0.1f; // 경직 시간
    [SerializeField] private float idleRanSt = 2.0f, idleRanEd = 7.0f, moveRanSt = 5.0f, moveRanEd = 9.0f;  // 랜덤 유휴 상태 시간 interval, 랜덤 이동 상태 시간 interval
    private float attackDelay = 3.5f;
    private float moveTime; // 움직인 시간
    private float stunTime; // 경직된 시간
    private float idleTime; // 유휴 상태 시간
    private float timer;    // 타이머
    public State state;
    public State CurrentState { get { return state; } set { state = value; } }
    public bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    private bool isTriggered;
    private bool isStunned = false;
    public bool IsStunned { get { return isStunned; } set { isStunned = value; } }
    private bool isDead = false;
    [SerializeField] private float directionChangeDelay = 1.0f;
    private bool canChangeDirection = true;
    [SerializeField] private PlayerDetector playerDetector;
    private EnemyAnimController anim;
    private WallDetector wallDetector;
    private PlatformDetector platformDetector;
    void Awake()
    {
        anim = GetComponent<EnemyAnimController>();
        playerDetector = GetComponent<PlayerDetector>();
        defaultLayer = gameObject.layer;
        wallDetector = GetComponent<WallDetector>();
        platformDetector = GetComponent<PlatformDetector>();
        SetIdleState();
    }
    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            // 죽었다면 아무 것도 하지 않음
            case State.Death:
                if (!isDead)
                {
                    isDead = true;
                    anim.Death();
                }
                return;
            case State.Idle:
                if (timer >= idleTime) // 랜덤 이동 시간이 끝나면 Move State 로 전환
                    SetMoveState();
                break;
            case State.Moving:
                // 움직이는 동안 바닥 체크, 벽 체크
                if (platformDetector.CheckPlatformEdge()) Flip();
                if (wallDetector.CheckFrontWall()) Flip(); ;
                if (timer >= moveTime)
                    SetIdleState();
                break;
            case State.Chasing:
                // 쫒는 동안 플레이어의 위치 방향과 현재 방향이 다르다면
                if (platformDetector.CheckPlatformEdge()) Flip();
                if (direction != playerDetector.PlayerDirection)
                {
                    if (!isAttacking)
                        // 방향 전환 딜레이 중이 아닐 때 방향 전환
                        if (canChangeDirection)
                        {
                            canChangeDirection = false;
                            Flip();
                            StartCoroutine(WaitNextDirectionChange());
                        }
                }
                // 플레이어가 공격 범위 내에 들어오면
                if (playerDetector.InAttackRange)
                {
                    // 공격 딜레이 중이 아니라면 공격
                    if (!isTriggered)
                    {
                        Attack();
                        StartCoroutine(AttackDelayRoutine());
                    }
                    // Idle 애니메이션 상태로 대기
                    else
                    {
                        anim.Idle();
                    }
                }
                // 공격 범위 밖이라면 쫒기(Move 애니메이션)
                else
                {
                    if (wallDetector.CheckFrontWall())
                        Flip();
                    anim.Move();
                }
                if (!playerDetector.InRange)
                    SetIdleState();
                break;
            case State.GetAway:
                if (platformDetector.CheckPlatformEdge()) Flip();
                if (!playerDetector.inRange) SetIdleState();
                else
                {
                    if(direction == playerDetector.playerDirection) Flip();
                }
                break;
        }
        if (playerDetector.InRange && state != State.Chasing)
        {
            SetChasingState();
        }
        if (isStunned)
        {
            stunTime += Time.deltaTime;
            if (stunTime >= stunDuration)
            {
                isStunned = false;
                stunTime = 0f;
            }
        }
    }
    private void Attack()
    {
        isAttacking = true;
        anim.Attack();
    }
    void SetIdleState()
    {
        state = State.Idle;
        timer = 0.0f;
        idleTime = Random.Range(idleRanSt, idleRanEd);
        anim.Idle();
    }
    void SetMoveState()
    {
        speed = basicSpeed;
        state = State.Moving;
        timer = 0.0f;
        moveTime = Random.Range(moveRanSt, moveRanEd);
        anim.Move();
    }
    void SetChasingState()
    {
        speed = maxSpeed;
        state = State.Chasing;
        anim.Move();
    }
    // 몬스터의 방향값 direction 과 sprite 이미지를 반전시킨다
    void Flip()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    // 플레이어를 감지하고 쫒는 상황 (Chasing State)에서 플레이어의 방향에 따라 몬스터가 방향 전환 할 때에 딜레이
    IEnumerator WaitNextDirectionChange()
    {
        yield return new WaitForSeconds(directionChangeDelay);
        canChangeDirection = true;
    }
    // 공격 딜레이
    IEnumerator AttackDelayRoutine()
    {
        isTriggered = true;
        yield return new WaitForSeconds(attackDelay);
        isTriggered = false;
    }
}