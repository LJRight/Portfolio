using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] GameObject shop;
    [SerializeField] float interval = 10f;
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if (time > interval)
        {
            time = 0f;
            Instantiate(shop, transform.position, Quaternion.identity, transform);
        }
    }


}
