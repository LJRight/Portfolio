using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackCollider : MonoBehaviour
{
    [SerializeField] private GameObject attackCollider; // 공격 범위에 사용할 Collider 객체
    private EnemyController enemy;
    private void Awake()
    {
        // 시작 시 공격 Collider 비활성화
        if (attackCollider != null)
            attackCollider.SetActive(false);
        enemy = GetComponent<EnemyController>();
    }
    private void Update()
    {
        if (!enemy.IsAttacking) DisableAttackCollider();
    }
    // 애니메이션 이벤트에서 호출할 함수
    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.SetActive(true);
        }
        ensuringDisableAttackCollider();
    }
    // 애니메이션 이벤트에서 호출할 함수
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.SetActive(false);
        }
    }

    // 플레이어 데미지 입는 함수 추가
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player")) // 플레이어인지 확인
    //     {
    //         PlayerController player = other.gameObject.GetComponent<PlayerController>();
    //         if (player != null)
    //         {
    //             player.OnPlayerDamaged(other);
    //         }
    //     }
    // }

    private IEnumerator ensuringDisableAttackCollider()
    {
        yield return new WaitForSeconds(1);
        DisableAttackCollider();
    }
}