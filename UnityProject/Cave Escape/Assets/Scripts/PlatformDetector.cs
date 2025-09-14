using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private float rayLength = 1f; // Ray 아래쪽 길이
    [SerializeField] private float rayFrontLength = 2.0f; // 몬스터 앞쪽 Ray 까지길이
    [SerializeField] private LayerMask groundLayer; // 바닥 감지 레이어
    [SerializeField] EnemyController enemy;

    /// <summary>
    /// 몬스터 약간 전방 아래쪽을 감지하여 플랫폼의 끝을 감지합니다
    /// </summary>
    /// <param name="direction">몬스터 이동 방향 (1: 오른쪽, -1: 왼쪽)</param>
    /// <returns>끝을 감지하면 true 반환</returns>
    public bool CheckPlatformEdge()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x + (enemy.Direction * rayFrontLength), transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);
        return hit.collider == null;
    }
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 RayOrigin = new Vector2(transform.position.x + (enemy.Direction * rayFrontLength), transform.position.y);
        Gizmos.DrawLine(RayOrigin, RayOrigin + Vector2.down * rayLength);

    }
}
