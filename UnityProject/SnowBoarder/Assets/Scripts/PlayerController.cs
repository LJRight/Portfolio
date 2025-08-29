using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1.0f;
    SurfaceEffector2D se2d;
    [SerializeField] float speedInterval = 0.75f;
    [SerializeField] float maxSpeed = 45.0f;
    public float normalSpeed = 10.0f;
    public float currentSpeed;
    Rigidbody2D rigid;
    bool canMove = true;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        se2d = FindObjectOfType<SurfaceEffector2D>();
    }
    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            RespondToBoost();
        }
    }

    public void DisableControls()
    {
        canMove = false;
    }
    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            rigid.AddTorque(torqueAmount);
        else if (Input.GetKey(KeyCode.RightArrow))
            rigid.AddTorque(-torqueAmount);
    }

    void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            se2d.speed = se2d.speed <= maxSpeed ? se2d.speed + speedInterval : maxSpeed;
        else
            se2d.speed = se2d.speed >= normalSpeed ? se2d.speed - speedInterval : normalSpeed;
        currentSpeed = se2d.speed;
    }
}
