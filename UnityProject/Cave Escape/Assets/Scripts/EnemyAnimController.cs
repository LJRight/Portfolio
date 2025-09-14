using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private EnemyController enemy;
    [SerializeField] private int idle = 0, move = 1, attack = 2;
    [SerializeField] private string actionParm = "Action", hitParm = "Hit", deadParm = "Dead", speedParm = "Move Speed";
    EnemyAudioController audioSource;
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<EnemyController>();
        audioSource = GetComponent<EnemyAudioController>();
    }
    public void Idle()
    {
        anim.SetInteger(actionParm, idle);
    }
    public void Move()
    {
        anim.SetInteger(actionParm, move);
        anim.SetFloat(speedParm, enemy.Speed / enemy.BasicSpeed); // EnemyController의 speed 값 기반으로 설정

    }
    public void Hit()
    {
        anim.SetTrigger(hitParm);
    }
    public void Attack()
    {
        anim.SetInteger(actionParm, attack);
        StartCoroutine(WaitAttack());
    }
    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        enemy.IsAttacking = false;
    }
    public void Death()
    {
        anim.SetTrigger(deadParm);
        StartCoroutine(WaitDeath());
    }
    private IEnumerator WaitDeath()
    {
        float deathTime = anim.GetCurrentAnimatorStateInfo(0).length > audioSource.Die ? anim.GetCurrentAnimatorStateInfo(0).length : audioSource.Die;
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}
