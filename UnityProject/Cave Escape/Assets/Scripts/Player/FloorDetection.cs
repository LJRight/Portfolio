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
        // �ٴ��� ���� ���ڱ� �Ҹ� ���� isWalking �߰�, blueLight ������ ���� �Ҹ� �� ���� (�̰� ���� ����)
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
