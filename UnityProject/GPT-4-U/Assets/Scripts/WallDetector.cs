using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] private Vector2 boxSize = new Vector2(1.0f, 4.8f);
    [SerializeField] private float wallCheckBoxXOffset = 2.5f, wallCheckBoxYOffset = 3.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] EnemyController enemy;
    
    /// <summary>
    /// 전방에 벽을 감지합니다.
    /// </summary>
    /// <param name="direction">몬스터 이동 방향 (1: 오른쪽, -1: 왼쪽)</param>
    /// <returns>벽을 감지하면 true 반환</returns>
    public bool CheckFrontWall()
    {

        Vector2 boxCenter = (Vector2)transform.position + new Vector2(enemy.Direction * wallCheckBoxXOffset, wallCheckBoxYOffset);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
        return hit != null;
    }

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(enemy.Direction * wallCheckBoxXOffset, wallCheckBoxYOffset);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
