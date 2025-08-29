using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetKey : MonoBehaviour
{
    Image keyImage;

    void Awake()
    {
        keyImage = GetComponent<Image>();
    }

    void Update()
    {
        if (GameManager.instance.hasKey)
        {
            keyImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            keyImage.color = new Color(1, 1, 1, 0.01f);
        }
    }
}
