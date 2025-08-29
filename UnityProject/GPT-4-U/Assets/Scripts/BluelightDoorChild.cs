using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluelightDoorChild : MonoBehaviour
{
    BluelightDoor BLD;
    void Awake()
    {
        BLD = GetComponentInParent<BluelightDoor>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bluelight"))
            BLD.OnChildTriggerEnter2D();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bluelight"))
            BLD.OnChildTriggerExit2D();
    }
}
