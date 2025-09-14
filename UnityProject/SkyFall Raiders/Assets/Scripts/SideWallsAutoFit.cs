using UnityEngine;

[ExecuteAlways]
public class SideWallsAutoFit : MonoBehaviour
{
    [SerializeField] Camera cam;          // 비워두면 MainCamera
    [SerializeField] float thickness = 0.5f;
    [SerializeField] Transform leftWall;  // BoxCollider2D 가진 오브젝트
    [SerializeField] Transform rightWall; // BoxCollider2D 가진 오브젝트
    [SerializeField] float extraTop = 2f, extraBottom = 2f; // 위/아래 여유
    void Reset() => cam = Camera.main;
    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!cam || !leftWall || !rightWall) return;

        // 카메라 월드 경계 계산
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        var cx = cam.transform.position.x;
        var cy = cam.transform.position.y;

        float minX = cx - halfWidth;
        float maxX = cx + halfWidth;
        float minY = cy - halfHeight - extraBottom;
        float maxY = cy + halfHeight + extraTop;
        float wallHeight = maxY - minY;

        // 왼쪽 벽
        leftWall.position = new Vector3(minX - thickness * 0.5f, (minY + maxY) * 0.5f, 0f);
        var lbox = leftWall.GetComponent<BoxCollider2D>();
        if (lbox) lbox.size = new Vector2(thickness, wallHeight);

        // 오른쪽 벽
        rightWall.position = new Vector3(maxX + thickness * 0.5f, (minY + maxY) * 0.5f, 0f);
        var rbox = rightWall.GetComponent<BoxCollider2D>();
        if (rbox) rbox.size = new Vector2(thickness, wallHeight);
    }
}