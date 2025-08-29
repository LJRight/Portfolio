using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootholdCheck : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Foothold"))
        {
            gameObject.transform.SetParent(other.gameObject.transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Foothold"))
        {
            gameObject.transform.SetParent(null);
        }
    }
}
