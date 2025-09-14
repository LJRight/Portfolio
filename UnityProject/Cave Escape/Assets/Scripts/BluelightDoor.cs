using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluelightDoor : MonoBehaviour
{
    SpriteRenderer render;
    Collider2D collider;
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    public void OnChildTriggerEnter2D()
    {
        collider.enabled = true;
        render.enabled = true;
    }
    public void OnChildTriggerExit2D()
    {
        collider.enabled = false;
        render.enabled = false;
    }
}
