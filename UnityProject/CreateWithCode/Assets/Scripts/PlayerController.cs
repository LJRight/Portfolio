using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravityModifier;
    [SerializeField] bool isOnGround;
    [SerializeField] ParticleSystem myParticle_1, myParticle_2;
    bool gameOver = false;
    public bool GameOver => gameOver;
    private Animator myAnim;
    [SerializeField] AudioClip jumpSound, crashSound;
    private AudioSource myAudio;
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            myParticle_2.Stop();
            myAnim.SetTrigger("Jump_trig");
            myAudio.PlayOneShot(jumpSound, 1.0f);
            m_rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
        myAnim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isOnGround = true;
                myParticle_2.Play();
                break;
            case "Obstacle":
                gameOver = true;
                Debug.Log("Game Over!");
                myParticle_2.Stop();
                myParticle_1.Play();
                myAudio.PlayOneShot(crashSound, 1.0f);
                myAnim.SetBool("Death_b", true);
                myAnim.SetInteger("DeathType_int", 1);
                break;
        }
    }


}
