using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrail : MonoBehaviour
{
    float simulateSpeed;
    [SerializeField] ParticleSystem dustTrail;
    PlayerController player;
    void Start()
    {
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        var main = dustTrail.main;
        main.simulationSpeed = player.currentSpeed / player.normalSpeed;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform")) ;
        {
            dustTrail.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform")) ;
        {
            dustTrail.Stop();
        }
    }
}

