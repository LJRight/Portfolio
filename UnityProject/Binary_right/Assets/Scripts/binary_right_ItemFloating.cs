using UnityEngine;

public class binary_right_ItemFloating : MonoBehaviour
{
    [SerializeField] float amplitude = .25f, frequency = 1f;
    Vector3 initPos;
    void Start()
    {
        initPos = transform.position;
    }
    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = initPos + new Vector3(0, offset, 0);
    }

}
