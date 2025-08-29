using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    private int[] points = { 75, 100, 150, 200 };
    private int myPoint;
    bool wasCollected = false;
    void Awake()
    {
        myPoint = points[Random.Range(0, 4)];
        transform.localScale *= myPoint / 100f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().GetCoin(myPoint);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}