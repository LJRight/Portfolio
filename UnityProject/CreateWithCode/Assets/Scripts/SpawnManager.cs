using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] Vector3 spawnPos = new Vector3(25, 0, 0);
    float intervalMin = 2.5f, intervalMax = 5.5f;
    private PlayerController player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (player.GameOver)
                break;
            Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
        }
    }
}
