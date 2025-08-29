using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Value")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpSpeed = 7f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float blinkTime = 0.25f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    String runAnimBool = "isRunning", ClimbAnimBool = "isClimbing";
    float defaultGravity;
    bool isAlive = true;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer renderer;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        defaultGravity = myRigidbody.gravityScale;
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder"))) return;
        if (value.isPressed)
        {
            myRigidbody.velocityY = jumpSpeed;
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocityY);
        myRigidbody.velocity = playerVelocity;

        myAnimator.SetBool(runAnimBool, !IsZero(myRigidbody.velocityX));
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = defaultGravity;
            myAnimator.SetBool(ClimbAnimBool, false);
            myAnimator.speed = 1f;
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocityX, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        myAnimator.SetBool(ClimbAnimBool, true);

        if (IsZero(myRigidbody.velocityY)) myAnimator.speed = 0f;
        else myAnimator.speed = 1f;
    }
    void FlipSprite()
    {
        if (!IsZero(myRigidbody.velocityX))
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocityX), 1f);
    }

    bool IsZero(float value)
    {
        return Mathf.Abs(value) < Mathf.Epsilon;
    }
    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            StartCoroutine(Blinking());
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    IEnumerator Blinking()
    {
        renderer.enabled = false;
        yield return new WaitForSeconds(blinkTime);
        renderer.enabled = true;
    }
}
