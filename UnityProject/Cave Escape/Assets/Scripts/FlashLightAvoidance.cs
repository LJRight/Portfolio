using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightAvoidance : MonoBehaviour
{
    private bool isInLight = false;
    private EnemyController enemy;
    [SerializeField] private float getAwayTime = 1.5f;

    private void Awake() {
        enemy = GetComponent<EnemyController>();
    }
    
    private void Update() {
        if(isInLight)
        {
            enemy.CurrentState = State.GetAway;
            isInLight = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("flashlight")) // "Light" 태그를 가진 오브젝트와 접촉하면
        {
            isInLight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            isInLight = false;
        }
    }
    IEnumerator GetAwayInterval()
    {
        yield return new WaitForSeconds(getAwayTime);
        enemy.CurrentState = State.Idle;
    }
}
