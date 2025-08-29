using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    List<Transform> wayPoints;
    int wayPointIndex = 0;
    float moveSpeed;
    public void Init(List<Transform> wayPoints, float moveSpeed)
    {
        GetComponent<Shooter>();
        this.wayPoints = wayPoints;
        this.moveSpeed = moveSpeed;
        GetComponent<Shooter>().StartFiring();
    }
    void Update()
    {
        FollowPath();
    }
    void FollowPath()
    {
        if (wayPointIndex < wayPoints.Count)
        {
            Vector3 targetPosition = wayPoints[wayPointIndex].position;
            float delta = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
            if (transform.position == targetPosition)
                wayPointIndex++;
        }
        else
            Destroy(gameObject);
    }
}
