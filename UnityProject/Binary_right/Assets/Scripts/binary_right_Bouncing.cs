using UnityEngine;

public class binary_right_Bouncing : MonoBehaviour
{
    Rigidbody myRb;
    AudioSource myAudio;
    [SerializeField] float bounceForce, downForce;

    // When using sound assets
    [Header("Sound Setting")]
    [SerializeField] AudioClip bouncingSFX;
    [SerializeField] float bouncingVolume;
    void Awake()
    {
        myRb = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        DownForce();
    }
    void DownForce()
    {
        myRb.AddForce(Vector3.down * downForce, ForceMode.Acceleration);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            myRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            // myAudio.PlayOneShot(bouncingSFX, bouncingVolume);
        }
    }
}
