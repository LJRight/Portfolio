using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour
{
    BoxCollider2D boxColl;

    bool canOpen;

    void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canOpen)
            {
                SoundManager.instance.PlaySFX(SoundManager.SFX.BoxOpen, 0.33f);

                if (this.CompareTag("SignPost"))
                {
                    GameManager.instance.talkId = 2000;
                    UIManager.instance.Action();

                    boxColl.enabled = false; // 중복 열기 금지
                }

                if (this.CompareTag("SignPost2"))
                {
                    GameManager.instance.talkId = 3000;
                    UIManager.instance.Action();

                    boxColl.enabled = false;
                }

                if (this.CompareTag("SignPost3"))
                {
                    GameManager.instance.talkId = 4000;
                    UIManager.instance.Action();

                    boxColl.enabled = false;
                }

                if (this.CompareTag("SignPost4"))
                {
                    GameManager.instance.talkId = 5000;
                    UIManager.instance.Action();

                    boxColl.enabled = false;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canOpen = false;
    }
}
