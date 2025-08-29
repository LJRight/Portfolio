using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float delay = 1.0f;
    [SerializeField] ParticleSystem crashEffect;
    AudioSource audio;
    bool isAlive;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        isAlive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platform") && isAlive)
        {
            isAlive = false;
            FindObjectOfType<PlayerController>().DisableControls();
            crashEffect.Play();
            audio.Play();
            Invoke("ReloadScene", delay);
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
