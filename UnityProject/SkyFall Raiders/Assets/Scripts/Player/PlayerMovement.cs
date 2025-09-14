using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.5f;
    [SerializeField] float paddingLeft, paddingRight, paddingTop, paddingBottom;
    [SerializeField] bool isJoystick = false;
    [Header("Sub Machine")]
    VariableJoystick joystick;
    [SerializeField] private Transform[] followers;
    private Vector2[] followerOffsets;
    Rigidbody2D rb;
    Animator playerAnimator;
    Vector2 rawInput;
    Vector2 minBounds, maxBounds;
    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // #if UNITY_ANDROID || UNITY_IOS
        //         isJoystick = true;
        //         joystick = UIManager.Instance.GetComponentInChildren<VariableJoystick>(true);
        // #else
        //     isJoystick = false;
        // #endif
        if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isJoystick = true;
            joystick = UIManager.Instance.GetComponentInChildren<VariableJoystick>(true);
        }
        else
        {
            isJoystick = false;
        }
    }
    void Start()
    {
        InitBounds();
        followerOffsets = new Vector2[followers.Length];
        for (int i = 0; i < followers.Length; i++)
        {
            if (followers[i] != null)
                followerOffsets[i] = followers[i].localPosition;
        }
    }
    void FixedUpdate()
    {
        Vector2 curInput = isJoystick ? joystick.Direction : rawInput;
        Vector2 desiredVelocity = curInput * moveSpeed;
        SetAnimation(curInput.x);
        Vector2 nextPos = rb.position + desiredVelocity * Time.fixedDeltaTime;

        float clampedX = Mathf.Clamp(nextPos.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        float clampedY = Mathf.Clamp(nextPos.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        Vector2 clampedPos = new Vector2(clampedX, clampedY);

        Vector2 correctedVelocity = (clampedPos - rb.position) / Time.fixedDeltaTime;
        rb.linearVelocity = correctedVelocity;

        for (int i = 0; i < followers.Length; i++)
        {
            if (followers[i] != null && followers[i].gameObject.activeSelf)
            {
                Vector2 targetPos = rb.position + followerOffsets[i];
                Vector2 clampedTargetPos = new Vector2(Mathf.Clamp(targetPos.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight),
                                                        Mathf.Clamp(targetPos.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop));
                followers[i].GetComponent<FollowerMove>().SetTarget(clampedTargetPos);
            }
        }
    }
    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }
    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }
    public void SetAnimation(float value)
    {
        int direction = value > 0 ? 1 : value < 0 ? -1 : 0;
        playerAnimator.SetInteger("Input", direction);
    }
}
