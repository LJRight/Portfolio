using UnityEngine;
using System.Collections.Generic;

public static class Geometry2D
{
    private static Camera cam = Camera.main;
    private static Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
    private static Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane));
    private static Rect worldRect = new Rect(bl.x, bl.y, tr.x - bl.x, tr.y - bl.y);

    // A->B 방향으로 나가서 worldRect(월드좌표의 사각형)과 처음 만나는 교점 C
    public static Vector2 RaycastToRect(Vector2 A, Vector2 B)
    {
        Vector2 d = B - A;
        float dx = d.x, dy = d.y;

        List<float> ts = new();

        // x = minX, maxX
        if (Mathf.Abs(dx) > Mathf.Epsilon)
        {
            float t1 = (worldRect.xMin - A.x) / dx;
            float t2 = (worldRect.xMax - A.x) / dx;
            // 해당 t에서의 y가 사각형 y범위 안인지 확인
            float y1 = A.y + t1 * dy;
            float y2 = A.y + t2 * dy;
            if (t1 >= 0 && y1 >= worldRect.yMin && y1 <= worldRect.yMax) ts.Add(t1);
            if (t2 >= 0 && y2 >= worldRect.yMin && y2 <= worldRect.yMax) ts.Add(t2);
        }

        // y = minY, maxY
        if (Mathf.Abs(dy) > Mathf.Epsilon)
        {
            float t3 = (worldRect.yMin - A.y) / dy;
            float t4 = (worldRect.yMax - A.y) / dy;
            float x3 = A.x + t3 * dx;
            float x4 = A.x + t4 * dx;
            if (t3 >= 0 && x3 >= worldRect.xMin && x3 <= worldRect.xMax) ts.Add(t3);
            if (t4 >= 0 && x4 >= worldRect.xMin && x4 <= worldRect.xMax) ts.Add(t4);
        }

        if (ts.Count == 0) return A; // 교점 없음(평행/밖으로 안나감 등)
        float t = Mathf.Max(ts.ToArray());
        return A + d * t;
    }
}