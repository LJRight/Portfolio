using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePulse : MonoBehaviour
{
    public float baseScale = 1.0f;
    public float amplitude = 0.2f;
    public float frequency = 2.0f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = baseScale + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
        transform.localScale = initialScale * scaleFactor;
    }
}
