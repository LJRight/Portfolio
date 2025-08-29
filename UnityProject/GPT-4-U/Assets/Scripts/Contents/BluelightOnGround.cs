using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BluelightOnGround : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵 참조
    private TilemapRenderer tilemapRenderer; // TilemapRenderer 참조
    private TilemapCollider2D tileCollider;
    public CircleCollider2D bluelightCollider; // Bluelight의 CircleCollider2D 참조

    private bool isTilemapVisible = false; // 현재 타일맵 상태

    void Awake()
    {
        // TilemapRenderer 및 TilemapCollider2D 참조
        if (tilemap != null)
        {
            tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
            tileCollider = tilemap.GetComponent<TilemapCollider2D>();
        }
    }

    void Update()
    {
        if (bluelightCollider != null && tilemap != null)
        {
            CheckTilemapVisibility();
        }
    }

    void CheckTilemapVisibility()
    {
        // Bluelight의 CircleCollider2D 범위와 충돌하는 타일 확인
        BoundsInt bounds = tilemap.cellBounds;

        bool hasTileInRange = false;
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.cellSize / 2;
            if (bluelightCollider.bounds.Contains(worldPos))
            {
                hasTileInRange = true;
                break;
            }
        }

        // 범위에 타일이 있으면 타일맵 보이기, 아니면 숨기기
        if (hasTileInRange && !isTilemapVisible)
        {
            ShowTilemap();
        }
        else if (!hasTileInRange && isTilemapVisible)
        {
            HideTilemap();
        }
    }

    void ShowTilemap()
    {
        // TilemapRenderer의 sortingOrder 값을 높게 설정하여 보이게 함
        if (tilemapRenderer != null && tilemapRenderer.sortingOrder == -1)
        {
            tilemapRenderer.sortingOrder = 2; // 원하는 레이어 순서로 설정
            if (tileCollider != null)
            {
                tileCollider.enabled = true;
            }
        }
        else if (tilemapRenderer != null && tilemapRenderer.sortingOrder == 2)
        {
            tilemapRenderer.sortingOrder = -1;
            if (tileCollider != null)
            {
                tileCollider.enabled = false;
            }
        }

        isTilemapVisible = true;
    }

    void HideTilemap()
    {
        // TilemapRenderer의 sortingOrder 값을 낮게 설정하여 숨김
        if (tilemapRenderer != null && tilemapRenderer.sortingOrder == 2)
        {
            tilemapRenderer.sortingOrder = -1; // 숨기기 위해 낮은 값 설정
            if (tileCollider != null)
            {
                tileCollider.enabled = false;
            }
        }
        else if (tilemapRenderer != null && tilemapRenderer.sortingOrder == -1)
        {
            tilemapRenderer.sortingOrder = 2;
            if (tileCollider != null)
            {
                tileCollider.enabled = true;
            }
        }

        isTilemapVisible = false;
    }
}
