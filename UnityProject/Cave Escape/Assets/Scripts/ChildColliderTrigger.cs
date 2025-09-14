using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderTrigger : MonoBehaviour
{
    private BlueLightWeaknesses parentScript;
    private bool isInBlueLight = false;

    void Awake()
    {
        parentScript = GetComponentInParent<BlueLightWeaknesses>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bluelight"))
        {
            if (!isInBlueLight)
            {
                Debug.Log("In BlueLIght");
                parentScript?.OnChildTriggerEnter2D();
                isInBlueLight = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bluelight"))
        {
            if (isInBlueLight)
            {
                Debug.Log("BlueLight Out");
                parentScript?.OnChildTriggerExit2D();
                isInBlueLight = false;
            }
        }
    }
    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(1);
        isInBlueLight = true;
    }
}