using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float knockbackPower = 10.0f;  // 피격 시 넉백되는 정도
    [SerializeField] float InvincibleTime = 1.0f;   // 피격 시 무적 시간
    bool isInvincible = false;  // 무적 여부
    [SerializeField] float speed;
    [SerializeField] float JumpPower;

    // ???? ???? ???? ???? (?????? ????????? 2?????? ????)
    [SerializeField] int JumpCount;
    [SerializeField] int maxJumpCount;
    [SerializeField] float jumpDistance;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public Animator animator;

    // lights
    public GameObject flashLight;
    public GameObject blueLight;

    [SerializeField] float blueLightPenalty;

    // Door (Test)
    public GameObject secretDoor;

    PlayerReposition playerRepos;
    FloorDetection floorDetection;

    bool isPlayerHit;

    //public UI_Life life;
    //public LightEnergy lightEnergy;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerRepos = GetComponent<PlayerReposition>();
        floorDetection = GetComponent<FloorDetection>();
        UI_Life.instance.Player = this; // 개발할 때 주석처리 할 것
    }

    void Update()
    {
        if (!GameManager.instance.isDead && !UIManager.instance.isAction && !GameManager.instance.isCliff)
        {
            Move();
            Jump();
            
            LightOnOff();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // spacebar ??? ????J ????????? UI ???
        {
            if (UIManager.instance.isAction)
            {
                UIManager.instance.Action();
            }
        }

        if (UIManager.instance.lightEnergy != null && GameManager.instance.stage >= 2)
        {
            if (blueLight.activeSelf)
            {
                UIManager.instance.lightEnergy.ReduceEnerygy();
                if (UIManager.instance.lightEnergy.EnergyBar.value <= 0.0f)
                {
                    blueLight.SetActive(!blueLight.activeSelf);
                    flashLight.SetActive(!flashLight.activeSelf);
                    speed += blueLightPenalty;
                }
            }
            else
            {
                UIManager.instance.lightEnergy.IncreaseEnergy();
            }
        }
    }

    void Move()
    {
        float MoveX = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(MoveX * speed, rigid.velocity.y);

        if (rigid.velocity.x == 0.0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        // X?? ???????? Flip Change
        if (MoveX < 0)
        {
            spriteRenderer.flipX = true;
            flashLight.transform.localPosition = new Vector3(-Mathf.Abs(flashLight.transform.localPosition.x), flashLight.transform.localPosition.y, flashLight.transform.localPosition.z);
            flashLight.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else if (MoveX > 0)
        {
            spriteRenderer.flipX = false;
            flashLight.transform.localPosition = new Vector3(Mathf.Abs(flashLight.transform.localPosition.x), flashLight.transform.localPosition.y, flashLight.transform.localPosition.z);
            flashLight.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && /*!animator.GetBool("isJumping")*/ JumpCount != 0) // ???? ???? ?? ??????? ???? X 
        {
            // ??????? ?????? ?? y?? 0???? ???? (??????? ???? ????)
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            animator.SetInteger("JumpCount", JumpCount);
            JumpCount--;

            // blueLight?? ?? ??? X
            if (blueLight.activeSelf == false)
                SoundManager.instance.PlaySFX(SoundManager.SFX.Jump, 0.2f);
        }

        // ???????? ??
        if (rigid.velocity.y < 0)
        {
            //Debug.DrawRay(rigid.position, Vector2.down * jumpDistance, Color.red, 1f);
            RaycastHit2D rayHit = Physics2D.CircleCast(rigid.position, 0.3f, Vector2.down, jumpDistance, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < jumpDistance)
                {
                    animator.SetBool("isJumping", false);
                    animator.SetInteger("JumpCount", JumpCount);
                }

                JumpCount = maxJumpCount;
            }
        }
    }

    void LightOnOff()
    {
        if (GameManager.instance.stage >= 2)
        {
            // Tab? ?????? Bluelight ON/OFF
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Use Toggle
                blueLight.SetActive(!blueLight.activeSelf);
                flashLight.SetActive(!flashLight.activeSelf);

                if (secretDoor != null)
                    secretDoor.SetActive(!secretDoor.activeSelf);

                if (blueLight.activeSelf)
                {
                    speed -= blueLightPenalty;
                }
                else
                {
                    speed += blueLightPenalty;
                }

                SoundManager.instance.PlaySFX(SoundManager.SFX.LightOnOff, 0.2f);
            }
        }
    }

    public void OnPlayerDead()
    {
        // ??????? ??? Dead ????
        if (GameManager.instance.isDead)
        {
            animator.SetTrigger("isDead");
            SoundManager.instance.PlaySFX(SoundManager.SFX.Dead, 0.2f);

            // ?????? ???????? OFF
            if (flashLight.activeSelf == true)
            {
                flashLight.SetActive(false);
            }

            GameManager.instance.GameOverScene();
        }
    }

    public void OnPlayerDamaged(Collision2D other)
    {
        isInvincible = true;
        StartCoroutine(InvincibleTimer());

        isPlayerHit = true;
        UI_Life.instance.DecreaseUILife();
        //life.DecreaseUILife();

        if (GameManager.instance.hp > 0)
        {
            animator.SetTrigger("isHit");
            SoundManager.instance.PlaySFX(SoundManager.SFX.PlayerHit, 0.8f);
        }
        else
        {
            OnPlayerDead();
        }
    }

    // Animation Event (??????? Hit Motion ???? ??)
    void OnPlayerDamageFinish()
    {
        isPlayerHit = false;
        animator.SetTrigger("isNotHit");
    }

    void OnCliff()
    {
        if (GameManager.instance.isCliff)
            return;

        GameManager.instance.isCliff = true;
        UI_Life.instance.DecreaseUILife();
        //life.DecreaseUILife();

        animator.SetTrigger("isDead");
        SoundManager.instance.PlaySFX(SoundManager.SFX.Dead, 0.2f);

        // ?????? ???????? OFF
        if (flashLight.activeSelf == true)
        {
            flashLight.SetActive(false);
        }

        // ???????? Player ??? (??? ????????)
        if (!GameManager.instance.isDead)
        {
            playerRepos.StartPlayerReposCoroutine();
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        // ?? Object ???? ??, ?????????? ???? ????
        if (other.gameObject.CompareTag("Cliff"))
        {
            OnCliff();
        }
        if ((other.gameObject.CompareTag("Trap") || other.gameObject.CompareTag("Monster")) && !isInvincible)
        {
            OnPlayerDamaged(other);
        }
    }
    IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
    }
}