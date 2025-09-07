using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer = false;
    [SerializeField] private float health = 50;
    public float HP => health;
    [SerializeField] int myPoint = 10, myCoin = 5;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] GameObject coin, powerUp;
    CameraShake cameraShake;
    Animator myAnim;
    List<Shooter> myShooter;
    Collider2D myCollider;
    PathFinder pathFinder;
    PlayerRevive playerRevive;
    PlayerMovement playerMovement;
    bool isHit = false;
    void Awake()
    {
        myAnim = GetComponent<Animator>();
        if (!isPlayer)
        {
            pathFinder = GetComponent<PathFinder>();
            myShooter = GetComponents<Shooter>().ToList();
            myCollider = GetComponent<Collider2D>();
        }
        else
        {
            playerMovement = GetComponent<PlayerMovement>();
            cameraShake = Camera.main.GetComponent<CameraShake>();
            playerRevive = GetComponent<PlayerRevive>();
        }
    }
    public void Init(float hp, int point, int coin)
    {
        myPoint = point;
        myCoin = coin;
        health = hp;
    }
    public void GetLife()
    {
        health++;
        UIManager.Instance.SetPlayerHealthText(health);
    }
    void OnEnable()
    {
        isHit = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayer && !isHit && other.CompareTag("Enemy"))
            isHit = true;
        else if (isHit)
            return;
        DamageDealer DD = other.GetComponent<DamageDealer>();
        if (DD != null)
        {
            TakeDamage(DD.Damage);
            PlayHitEffect(other.transform.position);
            ShakeCamera();
            DD.Hit();
        }
    }
    void ShakeCamera()
    {
        if (cameraShake != null && isPlayer)
            cameraShake.Play();
    }
    void TakeDamage(float dmg)
    {
        health -= dmg;
        if (isPlayer)
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        if (isPlayer || health <= 0)
            Die();
        else
            myAnim.SetTrigger("isHit");
    }
    void PlayHitEffect(Vector3 pos)
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect,
                                                pos,
                                                Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }
    void Die()
    {
        if (!isPlayer)
        {
            foreach (Shooter sh in myShooter)
                sh.enabled = false;
            AudioPlayer.Instance.PlayEnemyDamagedClip();
            ScoreKeeper.Instance.AddScore(myPoint);
            StartCoroutine(WaitForAIDeath());
        }
        else
        {
            playerMovement.enabled = false;
            UIManager.Instance.SetPlayerHealthText(health);
            AudioPlayer.Instance.PlayPlayerDamagedClip();
            if (health <= 0)
            {
                StartCoroutine(GameOver());
                return;
            }
            playerRevive.Revive();
        }
    }
    IEnumerator GameOver()
    {
        myAnim.SetTrigger("isDead");
        playerMovement.enabled = false;
        gameObject.GetComponent<PlayerShooterManager>().AllStop();
        yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.15f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        GameController.Instance.GameOver();
    }
    IEnumerator WaitForAIDeath()
    {
        if (pathFinder != null)
            pathFinder.enabled = false;
        myCollider.enabled = false;
        myAnim.SetTrigger("isDead");
        yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.35f);
        ItemGenerate();
        Sniper sniper = GetComponent<Sniper>();
        if (sniper != null)
            FindFirstObjectByType<EnemySpawner>().PositionClear(sniper.Index);
        Destroy(gameObject);
    }
    private void ItemGenerate()
    {
        if (ScoreKeeper.Instance.CheckPowerUpItemScore())
        {
            Instantiate(powerUp, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject instance = Instantiate(coin, transform.position, Quaternion.identity);
            instance.GetComponent<Coin>().SetMoneyValue(myCoin);
        }
    }
}
