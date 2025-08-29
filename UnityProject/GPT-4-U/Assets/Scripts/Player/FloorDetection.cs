using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    bool isFloor;

    public PlayerController player;

    private void Start()
    {
        player = gameObject.GetOrAddComponent<PlayerController>();
    }

    void FootStep()
    {
        // 바닥일 때만 발자국 소리 나게 isWalking 추가, blueLight 상태일 때는 소리 안 나게 (이건 추후 생각)
        if (isFloor && player.blueLight.activeSelf == false)
        {
            SoundManager.instance.PlaySFX(SoundManager.SFX.Run, 0.2f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = false;
        }
    }
}
