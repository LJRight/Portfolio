using System.Collections;
using UnityEngine;

public class binary_right_MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 end;
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 1f;
    Vector3 start;
    Vector3 currentTarget;
    bool isWaiting = false;

    void Start()
    {
        start = transform.position;
        currentTarget = end;
    }
    void Update()
    {
        if (isWaiting) return;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            StartCoroutine(WaitAndSwitchDirection());
        }
    }

    IEnumerator WaitAndSwitchDirection()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentTarget = (currentTarget == end) ? start : end;
        isWaiting = false;
    }
}
