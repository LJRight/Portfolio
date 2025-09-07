using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    private float speed;
    Rigidbody2D myRB;
    private bool isReflected = false;
    [SerializeField] private float reflectTime = 0.75f;
    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && !isReflected)
        {
            isReflected = true;
            StartCoroutine(WaitNextReflection());
            Debug.Log(GetReflection(collision));
            myRB.linearVelocity = GetReflection(collision);
        }
    }
    private IEnumerator WaitNextReflection()
    {
        yield return new WaitForSeconds(reflectTime);
        isReflected = false;
    }
    private Vector2 GetReflection(Collider2D collision)
    {
        return myRB.linearVelocity.magnitude * Vector2.Reflect(myRB.linearVelocity.normalized, collision.gameObject.GetComponent<ReflectionWall>().normalV);
    }
}
