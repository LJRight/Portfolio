using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float soundRange = 20.0f, maxSoundRange = 15.0f;
    [SerializeField] float delay = 1.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] AudioClip currentClip;
    [SerializeField] private AudioClip basic, move, attack, die, hit;
    [SerializeField] private float maxVolume = 0.6f;
    EnemyController enemy;
    public float Die
    {
        get { return die.length; }
    }
    private bool canPlay = true;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
        delay += math.max(basic.length, move.length);
    }
    void Update()
    {
        SetVolume();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, soundRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxSoundRange);
    }
    float InSoundRange()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);
        return distance;
    }
    void SetVolume()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);

        if (distance > soundRange)
            audioSource.volume = 0;
        else if (distance <= soundRange && distance > maxSoundRange)
            audioSource.volume = maxVolume * (1 - (distance - maxSoundRange) / (soundRange - maxSoundRange));
        else
            audioSource.volume = maxVolume;
    }

    IEnumerator WaitNextSound()
    {
        canPlay = false;
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }
    public void AttackSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(attack);
    }
    public void hitSound()
    {
        audioSource.PlayOneShot(hit);
    }
    public bool CanPlay()
    {
        return canPlay && !enemy.IsAttacking;
    }
    public void BasicSound()
    {
        if (CanPlay())
        {
            audioSource.PlayOneShot(basic);
            StartCoroutine(WaitNextSound());
        }
    }
    public void MoveSound()
    {
        if (CanPlay())
        {
            audioSource.PlayOneShot(move);
            StartCoroutine(WaitNextSound());
        }
    }
    public void DieSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(die);
    }

}
