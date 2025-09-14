using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    List<Animator> anims = new List<Animator>();
    AudioSource audio;
    float timer;
    [SerializeField] float delay = 2.5f;
    void Awake()
    {
        foreach (Transform child in transform)
        {
            Animator anim = child.GetComponent<Animator>();
            anims.Add(anim);
        }
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            timer = 0;
            foreach (Animator anim in anims)
                anim.SetTrigger("trigger");
            audio.Play();
        }
    }
}
