using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class binary_right_PlayerController : MonoBehaviour
{
    Vector2 rawInput;
    binary_right_PlayerRespawn m_respawn;
    [SerializeField] float moveForce = 5f, doubleJumpForce, highJumpForce;
    [SerializeField] List<Material> myMaterials;
    Rigidbody myRb;
    Transform mainCamera;
    ItemAbility myAbility = ItemAbility.None;
    MeshRenderer myMR;
    void Awake()
    {
        myRb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        m_respawn = GetComponent<binary_right_PlayerRespawn>();
        myMR = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Vector3 moveDirection = mainCamera.forward.normalized * rawInput.y + mainCamera.right.normalized * rawInput.x;
        moveDirection.y = 0;
        myRb.AddForce(moveDirection * moveForce, ForceMode.Acceleration);
    }
    public Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = mainCamera.forward.normalized * rawInput.y + mainCamera.right.normalized * rawInput.x;
        moveDirection.y = 0;
        return moveDirection * moveForce;
    }
    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed && myAbility != ItemAbility.None)
        {
            switch (myAbility)
            {
                case ItemAbility.Double_Jump:
                    DoubleJump();
                    break;
                case ItemAbility.High_Jump:
                    HighJump();
                    break;
            }
            myAbility = ItemAbility.None;
            ChangeMaterial((int)myAbility);
        }
    }
    void HighJump()
    {

        myRb.AddForce(Vector3.up * highJumpForce, ForceMode.Impulse);
        // If using a sound asset, play the sound at this point
    }
    void DoubleJump()
    {
        Vector3 jumpDirection = mainCamera.forward.normalized;
        jumpDirection.y = 0;

        myRb.AddForce(jumpDirection * doubleJumpForce, ForceMode.Impulse);
        // If using a sound asset, play the sound at this point
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("hazard"))
            m_respawn.Die();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coin"))
        {
            binary_right_ScoreKeeper.Instance.AddScore();
            // If using a sound asset, play the sound at this point
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("item"))
        {
            myAbility = other.GetComponent<binary_right_ItemAbility>().Ability;
            other.GetComponent<binary_right_ItemRespawn>().UseItem();
            ChangeMaterial((int)myAbility);
            // If using a sound asset, play the sound at this point
        }
    }
    void ChangeMaterial(int type)
    {
        myMR.material = myMaterials[type];
    }
}
