using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    EnemyController enemy;
    Rigidbody2D rigid;
    [SerializeField] private float gravity = 20.0f;
    void Awake()
    {
        enemy = GetComponent<EnemyController>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (enemy.CurrentState == State.Death)
        {
            rigid.gravityScale = gravity;
        }
    }
}
