using System.Collections;
using System.Collections.Generic;
using Lightbug.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UI_Life : MonoBehaviour
{
    Image[] lifes;

    [SerializeField]
    PlayerController player;
    public PlayerController Player { set { player = value; } }

    public static UI_Life instance;

    void Awake()
    {
        instance = this;
        lifes = GetComponentsInChildren<Image>();
    }

    public void DecreaseUILife()
    {
        if (GameManager.instance.hp >= 0)
        {
            GameManager.instance.hp--;
            lifes[GameManager.instance.hp].enabled = false; // �÷��̾� ü�� ������ŭ UI���� ����
        }

        if (GameManager.instance.hp == 0)
        {
            GameManager.instance.isDead = true;
            player.OnPlayerDead();
        }
    }

    public void IncreaseUILife()
    {
        // 풀피면 피 회복 X
        if (GameManager.instance.hp == 3)
            return;

        if (GameManager.instance.hp <= 2)
        {
            GameManager.instance.hp++;
            lifes[GameManager.instance.hp - 1].enabled = true;
        }
    }
    public void ResetUILife()
    {
        foreach (Image img in lifes)
            img.enabled = true;
    }
}
