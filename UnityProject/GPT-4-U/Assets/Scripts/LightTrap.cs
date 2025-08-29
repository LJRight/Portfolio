using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "flashlight")
        {
            wall.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "flashlight")
        {
            wall.SetActive(true);
        }
    }

}
