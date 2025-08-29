using System.Collections;
using UnityEngine;

public class binary_right_PlayerRespawn : MonoBehaviour
{
    AudioSource myAudio;
    Rigidbody myRb;

    // When using sound assets
    [Header("Sound Setting")]
    [SerializeField] AudioClip crashingSFX;
    [SerializeField] float crashingVolume;

    [Header("Crash Effect")]
    [SerializeField] ParticleSystem crashingEffect;
    [SerializeField] float effectsDuration = 3.5f;
    [Header("Respawn setting")]
    [SerializeField] Vector3 respawnPos;
    void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
    }
    public void Die()
    {
        myRb.isKinematic = true;
        myRb.linearVelocity = Vector3.zero;
        myRb.angularVelocity = Vector3.zero;
        ActiveComponent(false);

        // myAudio.PlayOneShot(crashingSFX, crashingVolume);

        ParticleSystem instance = Instantiate(crashingEffect, transform.position, crashingEffect.transform.rotation);
        instance.Play();
        Destroy(instance.gameObject, effectsDuration);
        StartCoroutine(WaitEffect());

    }
    void ActiveComponent(bool option)
    {
        GetComponent<MeshRenderer>().enabled = option;
        GetComponent<binary_right_PlayerController>().enabled = option;
        GetComponent<binary_right_Bouncing>().enabled = option;

    }
    IEnumerator WaitEffect()
    {
        yield return new WaitForSeconds(effectsDuration);
        Respawn();
    }
    void Respawn()
    {
        transform.position = respawnPos;
        ActiveComponent(true);
        myRb.isKinematic = false;
        binary_right_ItemRespawn[] items = FindObjectsByType<binary_right_ItemRespawn>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.Respawn();
        }
    }
}
