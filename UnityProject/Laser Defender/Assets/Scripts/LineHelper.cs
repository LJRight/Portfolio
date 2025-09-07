using UnityEngine;
using System.Collections.Generic;

public static class LineHelper
{
    private enum SpawnPosition { Top, LeftSide, RightSide }
    private static float offset = 0.05f;   // 화면 밖으로 조금 벗어나게
    private static float yMinBound = 0.75f; // 좌우 스폰 시 Y 최소(화면 위쪽만)

    /// <summary>
    /// 화면 가장자리(위/좌/우)에서 시작하여 플레이어 방향으로 뻗어
    /// 화면 경계까지 이어지는 선의 (시작, 끝) 월드 좌표를 반환
    /// </summary>
    public static (Vector2 startPos, Vector2 endPos) GetRandomLineToPlayer(Transform player)
    {
        var cam = Camera.main;

        // 1) 스폰 위치(뷰포트) 결정
        SpawnPosition side = (SpawnPosition)Random.Range(0, System.Enum.GetValues(typeof(SpawnPosition)).Length);

        float vx = 0f, vy = 0f; // viewport x,y
        switch (side)
        {
            case SpawnPosition.Top:
                vy = 1f + offset;
                vx = Random.Range(0f - offset, 1f + offset);
                break;
            case SpawnPosition.LeftSide:
                vx = 0f - offset;
                vy = Random.Range(yMinBound, 1f + offset);
                break;
            case SpawnPosition.RightSide:
                vx = 1f + offset; // ← 오타 수정: 0f + offset 아님
                vy = Random.Range(yMinBound, 1f + offset);
                break;
        }

        // 2) 뷰포트 → 월드(z=0 평면에 맞춰 배치)
        float desiredWorldZ = 0f;
        float distFromCamToPlane = Mathf.Abs(desiredWorldZ - cam.transform.position.z);
        Vector3 startWorld3 = cam.ViewportToWorldPoint(new Vector3(vx, vy, distFromCamToPlane));
        startWorld3.z = desiredWorldZ; // 보장
        Vector2 startPos = startWorld3;

        // 3) 방향(월드)
        Vector2 dir = ((Vector2)player.position - startPos).normalized;
        if (dir.sqrMagnitude < 1e-6f) dir = Vector2.down; // 플레이어와 동일한 점인 극소수 케이스 보호

        // 4) 화면 경계(월드)
        Vector3 bl3 = cam.ViewportToWorldPoint(new Vector3(0, 0, distFromCamToPlane));
        Vector3 tr3 = cam.ViewportToWorldPoint(new Vector3(1, 1, distFromCamToPlane));
        float minX = bl3.x, maxX = tr3.x, minY = bl3.y, maxY = tr3.y;

        // 5) 직선 P(t) = start + dir * t 와 사각 경계의 교차 후보 t 계산(앞방향: t>0)
        var candidates = new List<float>(4);
        if (Mathf.Abs(dir.x) > 1e-6f)
        {
            float txMin = (minX - startPos.x) / dir.x;
            float txMax = (maxX - startPos.x) / dir.x;
            if (txMin > 0) candidates.Add(txMin);
            if (txMax > 0) candidates.Add(txMax);
        }
        if (Mathf.Abs(dir.y) > 1e-6f)
        {
            float tyMin = (minY - startPos.y) / dir.y;
            float tyMax = (maxY - startPos.y) / dir.y;
            if (tyMin > 0) candidates.Add(tyMin);
            if (tyMax > 0) candidates.Add(tyMax);
        }

        // 6) 가장 가까운(최소) 양수 t 선택
        if (candidates.Count == 0)
        {
            // 이론상 거의 없음(방향 성분이 모두 0). 안전망으로 대충 멀리 뻗기
            return (startPos, startPos + dir * 1000f);
        }
        float tEnd = Mathf.Min(candidates.ToArray());
        Vector2 endPos = startPos + dir * tEnd;

        return (startPos, endPos);
    }
}