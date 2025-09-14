using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedWidthCamera : MonoBehaviour
{
    public static FixedWidthCamera Instance { get; private set; }
    [SerializeField] private float worldWidth = 6f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        var cam = GetComponent<Camera>();
        cam.orthographic = true;
        
        // aspect 비율 기반으로 orthographicSize 계산
        cam.orthographicSize = worldWidth / cam.aspect * 0.5f;
        DontDestroyOnLoad(gameObject);
    }
}