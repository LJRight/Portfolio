using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightDetection : MonoBehaviour
{
    private TilemapRenderer tr;
    private TilemapCollider2D tc;

    void Start()
    {
        tr = GetComponent<TilemapRenderer>();
        tc = GetComponent<TilemapCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "flashlight")
        {
            tr.enabled = true;
            tc.isTrigger = false;
            Debug.Log("Player detected by flashlight");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "flashlight")
        {
            tr.enabled = false;
            tc.isTrigger = true;
        }
    }
}