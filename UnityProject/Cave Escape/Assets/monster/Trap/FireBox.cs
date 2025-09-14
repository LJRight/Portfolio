using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBox : MonoBehaviour
{
    Animator anim;
    AudioSource audio;
    [SerializeField] GameObject fire;
    [SerializeField] float delay = 2.5f;
    [SerializeField] float startTime;
    float timer;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        delay += audio.clip.length;
        fire.SetActive(false);
        if (startTime != 0)
            timer -= startTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            timer = 0f;
            anim.SetTrigger("Trigger");
        }
    }
    public void Fire()
    {
        fire.SetActive(true);
        audio.Play();
        StartCoroutine(WaitFire());
    }
    IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(audio.clip.length);
        fire.SetActive(false);
    }
}
