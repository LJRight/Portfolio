using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxColl;

    public GetPieces Pieces;

    [SerializeField]
    bool canOpen;

    void Awake()
    {
        animator = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canOpen)
            {
                animator.SetBool("IsOpened", true);
                SoundManager.instance.PlaySFX(SoundManager.SFX.BoxOpen, 0.33f);

                if (this.CompareTag("KeyBox"))
                {
                    GameManager.instance.talkId = 1000;
                    GameManager.instance.hasKey = true;
                    UIManager.instance.Action();

                    boxColl.enabled = false; // 중복 열기 금지
                }
                
                if (this.CompareTag("PieceBox"))
                {
                    Pieces.HidePieceImages();
                    Pieces.ShowPieceImages();
                    GameManager.instance.talkId = 10000 * (GameManager.instance.currentPiece + 1); // 현재 조각별로 talkId 수정
                    GameManager.instance.currentPiece++;
                    UIManager.instance.Action();

                    boxColl.enabled = false; // 중복 열기 금지
                }

                if (this.CompareTag("HpBox"))
                {
                    GameManager.instance.talkId = 200;
                    UIManager.instance.Action();
                    UI_Life.instance.IncreaseUILife();
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
