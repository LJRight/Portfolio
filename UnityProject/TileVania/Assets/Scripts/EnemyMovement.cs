using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    BoxCollider2D wallChecker;
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        wallChecker = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        myRigidbody.velocityX = moveSpeed;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        Flip();

    }
    void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
