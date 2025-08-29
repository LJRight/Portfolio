using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    [SerializeField] Vector2 revivePos, startPos;
    [SerializeField] float moveDuration = 2f, blinkInterval = 0.15f, RespawnDelay = 1.0f, invincibleDuration = 1.5f, offset = 0.85f;
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
    }
    public void Revive()
    {
        DisableComponent();
        StartCoroutine(WaitForDeathAnimation());
    }

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
        yield return new WaitForSeconds(invincibleDuration - offset);
        myHealth.enabled = true;
        myCollider.enabled = true;
    }
    void RespawnPlayer()
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
    void StartBlinkEffect()
    {
        sprite.DOFade(0.15f, blinkInterval)
            .SetLoops(Mathf.RoundToInt((moveDuration + invincibleDuration) / (blinkInterval * 2)), LoopType.Yoyo)
            .OnComplete(() => sprite.DOFade(1f, blinkInterval));
    }
    IEnumerator WaitForDeathAnimation()
    {
        myAnim.SetTrigger("isDead");
        yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.15f);
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
