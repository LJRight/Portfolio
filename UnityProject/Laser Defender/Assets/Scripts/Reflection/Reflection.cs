using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    Vector2 direction;
    float speed;
    Rigidbody2D myRB;
    int reflectionCount = 2;
    bool isReflected = false;
    float reflectTime = 0.75f;
    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && reflectionCount > 0 && !isReflected)
        {
            reflectionCount--;
            isReflected = true;
            StartCoroutine(WaitNextReflection());
            direction = myRB.velocity.normalized;
            speed = myRB.velocity.magnitude;
            direction = Vector2.Reflect(direction, collision.gameObject.GetComponent<ReflectionWall>().normalV);
            myRB.velocity = direction * speed;
        }
    }
    IEnumerator WaitNextReflection()
    {
        yield return new WaitForSeconds(reflectTime);
        isReflected = false;
    }
}
