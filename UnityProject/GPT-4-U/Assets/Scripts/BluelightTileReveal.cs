using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BluelightTileReveal : MonoBehaviour
{
    public Tilemap tilemap;              // 타일맵 레퍼런스
    TilemapCollider2D tc2D;
    public GameObject bluelight;         // bluelight GameObject
    private CircleCollider2D lightCollider; // bluelight의 CircleCollider2D
    public GameObject player;
    private HashSet<Vector3Int> revealedTiles = new HashSet<Vector3Int>(); // 이미 보인 타일 저장

    void Start()
    {
        // player = GameManager.instance.player;
        tc2D = tilemap.GetComponent<TilemapCollider2D>();

        lightCollider = bluelight.GetComponent<CircleCollider2D>();
        if (lightCollider == null)
        {
            Debug.LogError("CircleCollider2D가 bluelight에 없습니다!");
        }

        // 처음에 모든 타일 숨기기
        HideAllTilesOnStart();
    }

    void Update()
    {
        //Debug.Log(player.GetComponentInChildren<BlueLight>(true).ReturnSelfActive());
        // bluelight가 활성화 상태일 때만 타일을 처리
        if (player.GetComponentInChildren<BlueLight>(true).ReturnSelfActive())
        {
            RevealTilesWithinLight();
        }
        else if (revealedTiles.Count > 0)
        {
            // bluelight가 비활성화되었을 경우, 모든 타일을 숨김
            HideAllTiles();
        }
    }

    void HideAllTilesOnStart()
    {
        // 타일맵의 모든 타일을 숨기기
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                tilemap.SetTileFlags(pos, TileFlags.None); // 타일 플래그 제거
                tilemap.SetColor(pos, new Color(1, 1, 1, 0)); // 타일을 투명하게 설정

                tc2D.enabled = false;
            }
        }
    }

    void RevealTilesWithinLight()
    {
        // CircleCollider2D의 중심과 반지름 가져오기
        Vector3 lightPosition = bluelight.transform.position;
        float lightRadius = lightCollider.radius * bluelight.transform.lossyScale.x;

        // 타일맵의 전체 좌표 중에서 충돌 영역 안에 있는 타일 찾기
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3 worldPosition = tilemap.CellToWorld(pos) + tilemap.cellSize / 2; // 타일의 중심 좌표
            float distance = Vector3.Distance(lightPosition, worldPosition);

            if (distance <= lightRadius)
            {
                // 닿은 타일 활성화
                if (!revealedTiles.Contains(pos))
                {
                    revealedTiles.Add(pos);
                    RevealTile(pos);
                }
            }
            else
            {
                // 닿지 않은 타일 비활성화
                if (revealedTiles.Contains(pos))
                {
                    HideTile(pos);
                    revealedTiles.Remove(pos);
                }
            }
        }
    }

    void RevealTile(Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);
        if (tile != null)
        {
            tilemap.SetTileFlags(tilePosition, TileFlags.None); // 플래그 제거
            tilemap.SetColor(tilePosition, Color.white);
            tc2D.enabled = true;      // 타일 색상 설정
        }
    }

    void HideTile(Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);
        if (tile != null)
        {
            tilemap.SetTileFlags(tilePosition, TileFlags.None);
            tilemap.SetColor(tilePosition, new Color(1, 1, 1, 0)); // 투명하게 설정
            tc2D.enabled = false;
        }
    }

    void HideAllTiles()
    {
        // bluelight가 비활성화되었을 때 모든 타일을 숨김
        foreach (var pos in revealedTiles)
        {
            HideTile(pos);
        }
        revealedTiles.Clear(); // 숨긴 타일 목록 초기화
    }
}
