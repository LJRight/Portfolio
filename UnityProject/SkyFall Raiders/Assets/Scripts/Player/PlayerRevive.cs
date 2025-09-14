using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    [Header("Respawn (viewport coords 0~1)")]
    [SerializeField] Vector2 vpStart = new Vector2(0.5f, -0.10f); // 화면 아래 살짝 밖
    [SerializeField] Vector2 vpRevive = new Vector2(0.5f, 0.15f); // 화면 안 15% 지점
    private Vector2 revivePos, startPos;
    [SerializeField] float moveDuration = 2f, blinkInterval = 0.15f, RespawnDelay = 1.0f, invincibleDuration = 1.5f, invincibleDurationoffset = 0.85f;
    [SerializeField] float animationDelayOffset = .15f;
    PlayerMovement playerMovement;
    PlayerShooterManager playerShooterManager;
    Health myHealth;
    CapsuleCollider2D myCollider;
    Animator myAnim;
    SpriteRenderer sprite;
    bool isBlinking = false;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooterManager = GetComponent<PlayerShooterManager>();
        myHealth = GetComponent<Health>();
        myCollider = GetComponent<CapsuleCollider2D>();
        myAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        RecomputeWorldRespawnPositions();
    }
    void Start()
    {
        transform.position = revivePos;
    }
    void RecomputeWorldRespawnPositions()
    {
        var cam = Camera.main;
        startPos = cam.ViewportToWorldPoint(new Vector3(vpStart.x, vpStart.y, cam.nearClipPlane));
        revivePos = cam.ViewportToWorldPoint(new Vector3(vpRevive.x, vpRevive.y, cam.nearClipPlane));
    }

    public void Revive()
    {
        DisableComponent();
        StartCoroutine(WaitForDeathAnimation());
    }

    // Disable Player Component
    void DisableComponent()
    {
        playerMovement.enabled = false;
        myHealth.enabled = false;
        playerShooterManager.AllStop();
        myCollider.enabled = false;
    }
    IEnumerator EnableComponent()
    {
        playerMovement.enabled = true;
        playerShooterManager.ReFire();
        yield return new WaitForSeconds(invincibleDuration - invincibleDurationoffset);
        myHealth.enabled = true;
        myCollider.enabled = true;
    }
    private void RespawnPlayer()
    {
        transform.position = startPos;
        myAnim.SetInteger("Input", 0);
        isBlinking = false;
        transform.DOMove(revivePos, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() =>
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    StartBlinkEffect();
                }
            })
            .OnComplete(() =>
            {
                StartCoroutine(EnableComponent());
            });
    }
    private void StartBlinkEffect()
    {
        sprite.DOFade(0.15f, blinkInterval)
            .SetLoops(Mathf.RoundToInt((moveDuration + invincibleDuration) / (blinkInterval * 2)), LoopType.Yoyo)
            .OnComplete(() => sprite.DOFade(1f, blinkInterval));
    }
    IEnumerator WaitForDeathAnimation()
    {
        myAnim.SetTrigger("isDead");
        yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length + animationDelayOffset);
        transform.position = startPos;
        List<FollowerMove> followers = transform.GetComponentsInChildren<FollowerMove>().ToList();
        foreach (FollowerMove f in followers)
        {
            f.transform.position = startPos;
            f.SetTarget(startPos);
        }
        yield return new WaitForSeconds(RespawnDelay);
        RespawnPlayer();
    }

}
