using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    private EnemyController enemy;
    private EnemyAnimController anim;
    float InvincibleTime = 1.0f;   // 피격 시 무적 시간
    bool isInvincible = false;  // 무적 여부
    public bool IsInvincible { get { return isInvincible; } }
    void Awake()
    {
        enemy = GetComponent<EnemyController>();
        anim = GetComponent<EnemyAnimController>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap"))
            if (!isInvincible) TakeDamage();
    }
    public void TakeDamage()
    {
        enemy.HP = 1;
        if (enemy.HP > 0)
        {
            anim.Hit();
            enemy.IsStunned = true;
            isInvincible = true;
            StartCoroutine(InvincibleTimer());
        }
        else
        {
            anim.Death();
            enemy.CurrentState = State.Death;
        }
    }
    IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
    }
}