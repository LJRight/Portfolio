using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool hasPackage = false;
    [SerializeField] Color32 hasPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 noPackageColor = new Color32(50, 30, 20, 1);
    [SerializeField] float destroyDelay = 1.0f;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Fucking hurts... Damn");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package") && !hasPackage)
        {
            Debug.Log("Package picked up");
            spriteRenderer.color = other.gameObject.GetComponent<SpriteRenderer>().color;
            Destroy(other.gameObject, destroyDelay);
            hasPackage = true;
        }
        if (other.CompareTag("Customer") && hasPackage)
        {
            Debug.Log("Delivery Complete!");
            spriteRenderer.color = noPackageColor;
            hasPackage = false;
        }
    }
}
