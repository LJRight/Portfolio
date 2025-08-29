using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Rigidbody2D rigid;
    private EnemyController enemy;
    private PlayerDetector detector;
    private State state;
    private bool isAttacking;
    private bool isStunned;
    private bool canAttack;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyController>();
        detector = GetComponent<PlayerDetector>();
    }
    void Update()
    {
        state = enemy.CurrentState;
        isAttacking = enemy.IsAttacking;
        isStunned = enemy.IsStunned;
        canAttack = detector.InAttackRange;
    }
    void FixedUpdate()
    {
        if (state == State.Death || isStunned || isAttacking || state == State.Idle || canAttack)
            Stop();
        else if (state == State.Moving || state == State.Chasing || state == State.GetAway)
            Move(); // 이동 중이라면 이동 처리
    }
    void Move()
    {
        rigid.velocity = new Vector2(enemy.Direction * enemy.Speed, rigid.velocity.y);
    }
    void Stop()
    {
        rigid.velocity = Vector2.zero;
    }
}
