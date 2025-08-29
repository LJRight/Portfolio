using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float angleRange = 45f;
    [SerializeField] bool randomDirection = true;
    Rigidbody2D rb;
    Vector2 direction;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        CheckCameraBound();
    }
    void Start()
    {
        if (randomDirection)
            direction = GetRandomDirectionAroundDown();
        else
            direction = new Vector2(0f, -1f);
        rb.velocity = direction * speed;
    }
    Vector2 GetRandomDirectionAroundDown()
    {
        return (Quaternion.Euler(0, 0, Random.Range(-angleRange, angleRange)) * -transform.up).normalized;
    }
    void CheckCameraBound()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.05f || viewportPos.x > 1.05f || viewportPos.y < -0.08f || viewportPos.y > 1.08f)
            Destroy(gameObject);
    }
}
