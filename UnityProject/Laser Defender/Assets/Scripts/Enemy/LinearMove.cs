using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMove : MonoBehaviour
{
    private Vector2 targetPos;
    private float speed;
    public void Init(Vector2 targetPos, float speed)
    {
        
        this.targetPos = targetPos;
        Debug.Log($"Init Method Run!\nTarget Position : {targetPos}");
        this.speed = speed;
    }
    void Update()
    {
        if (targetPos != null && !IsMoveCompleted())
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        else
            Destroy(gameObject);
    }
    private bool IsMoveCompleted()
    {
        return Vector3.Distance(transform.position, targetPos) < 0.1f;
    }
}
