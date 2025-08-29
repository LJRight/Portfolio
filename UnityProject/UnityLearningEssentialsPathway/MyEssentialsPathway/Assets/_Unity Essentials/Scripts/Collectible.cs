using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject particle;
    void Start()
    {

    }
    void Update()
    {
        transform.Rotate(0, .3f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
