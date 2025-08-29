using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    Animator anim;
    BoxCollider2D collider;
    AudioSource audio;
    float timer;
    [SerializeField] float delay = 2.5f;
    [SerializeField] float activeTime = 1.5f;
    [SerializeField] private Vector2 boxSize = new Vector2(1.0f, 4.8f);
    Vector2 boxCenter;
    [SerializeField] float boxCenterOffSet;
    [SerializeField] private LayerMask playerLayer;
    bool CanActive = true;
    AudioClip originalClip;
    AudioClip reversedClip;
    private void Awake()
    {
        float z = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        boxCenter = (Vector2)transform.position + new Vector2(0f, boxCenterOffSet);
        audio = GetComponent<AudioSource>();
        originalClip = audio.clip;
        reversedClip = ReverseClip(originalClip);
        boxCenter = (Vector2)transform.position + new Vector2(Mathf.Sin(z) * -1 * boxCenterOffSet, Mathf.Cos(z) * boxCenterOffSet);
        if (Mathf.Abs(Mathf.Sin(z)) == 1)
            boxSize = new Vector2(boxSize.y, boxSize.x);
    }

    private void Update()
    {
        if (ActiveDetect() && CanActive)
            ActiveTrap();
    }
    public bool ActiveDetect()
    {
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, playerLayer);
        return hit != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    void ActiveTrap()
    {
        anim.SetTrigger("Up");
        StartCoroutine(WaitActiveTime());
        CanActive = false;
        audio.clip = originalClip;
        audio.Play();
    }
    void TrapEnd()
    {
        anim.SetTrigger("Down");
        StartCoroutine(WaitNextTrapDelay());
        audio.clip = reversedClip;
        audio.Play();
    }
    IEnumerator WaitActiveTime()
    {
        yield return new WaitForSeconds(activeTime);
        TrapEnd();
    }
    IEnumerator WaitNextTrapDelay()
    {
        yield return new WaitForSeconds(delay);
        CanActive = true;
    }
    public void ColliderOn()
    { collider.enabled = true; }
    public void ColliderOff()
    { collider.enabled = false; }
    private AudioClip ReverseClip(AudioClip clip)
    {
        int sampleCount = clip.samples * clip.channels;
        float[] samples = new float[sampleCount];
        clip.GetData(samples, 0);

        float[] reversedSamples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            reversedSamples[i] = samples[sampleCount - 1 - i];
        }
        AudioClip reversedClip = AudioClip.Create(clip.name + "_Reversed", clip.samples, clip.channels, clip.frequency, false);
        reversedClip.SetData(reversedSamples, 0);
        return reversedClip;
    }
}