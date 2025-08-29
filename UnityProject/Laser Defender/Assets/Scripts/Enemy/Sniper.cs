using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    Vector2 targetPos;
    [SerializeField] float turnSpeed = 90f;
    float shootingDelay = 1.5f;
    float speed;
    int myIdx;
    public int Index => myIdx;
    bool isMoving, isAiming;
    const float SqrEPS = 0.1f;
    const float SPRITE_FACING_OFFSET = -90f;
    void Start()
    {
        isMoving = true;
        isAiming = false;
    }
    void Update()
    {
        if (isMoving)
            Move();
        else
        {
            if (!isAiming)
            {
                isAiming = true;
                StartCoroutine(WaitBeforeShooting());
            }
            Aiming();
        }
    }
    //Function to initialize the characteristics of the Sniper enemy aircraft when instantiating
    public void Init(Vector2 target, int idx, float moveSpeed)
    {
        targetPos = target;
        myIdx = idx;
        speed = moveSpeed;
    }
    // Function to move to the target location received from EnemySpawner
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (((Vector2)transform.position - targetPos).sqrMagnitude <= SqrEPS)
            isMoving = false;
    }
    // A function that continuously rotates the front of the enemy aircraft toward the player
    private void Aiming()
    {
        Transform playerT = FindFirstObjectByType<PlayerMovement>()?.transform;
        if (playerT == null)
        {
            return;
        }
        Vector3 aimDir = transform.position - playerT.transform.position;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg + SPRITE_FACING_OFFSET);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }
    //A coroutine that gives a slight delay before starting the attack after arriving at the target point
    IEnumerator WaitBeforeShooting()
    {
        yield return new WaitForSeconds(shootingDelay);
        GetComponent<Shooter>().StartFiring();
    }
}
