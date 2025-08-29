using UnityEngine;

public class binary_right_PlayerPos : MonoBehaviour
{
    public Transform rayOrigin;
    public float rayDistance = 10f;
    public LayerMask groundMask;
    public float heightOffset = 6.0f;
    void LateUpdate()
    {
        if (Physics.Raycast(rayOrigin.position, Vector3.down, out RaycastHit hit, heightOffset, groundMask))
            transform.position = hit.point + Vector3.up * heightOffset;
    }
}

