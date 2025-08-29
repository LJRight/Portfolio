using UnityEngine;

public class binary_right_CoinRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
