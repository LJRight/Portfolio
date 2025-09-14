using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReposition : MonoBehaviour
{
    public PlayerController player;

    public void StartPlayerReposCoroutine()
    {
        StartCoroutine(PlayerReposCoroutine());
    }

    IEnumerator PlayerReposCoroutine()
    {
        // �ڷ�ƾ�� ����Ͽ� 2�� �Ŀ� ��Ȱ
        yield return new WaitForSeconds(2.0f);

        PlayerRepos();
        player.animator.SetTrigger("isLive");
        SoundManager.instance.PlaySFX(SoundManager.SFX.Live, 0.2f);

        // 0.6�� �Ŀ� ������ �� �ְ� ����
        yield return new WaitForSeconds(0.6f);
        player.animator.SetTrigger("CanMove");
        GameManager.instance.isDead = false;

        // ��Ȱ�ϸ� ������ �ٽ� ON
        if (player.flashLight.activeSelf == false && player.blueLight.activeSelf == false)
        {
            player.flashLight.SetActive(true);
            GameManager.instance.isCliff = false;
        }
    }

    void PlayerRepos()
    {
        // Test��, GameManager���� �迭�� ���������� reposition ��ġ ������ ����
        player.transform.position = new Vector3(-6.76f, -1.141f, 0.0f);
        player.animator.SetBool("isJumping", false);
    }
}
