using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightWeaknesses : MonoBehaviour
{
    private bool isInLight = false;
    private EnemyController enemy;
    private EnemyAnimController anim;
    private ApplyDamage dmg;
    [SerializeField] private float speedDecreaseInterval = 0.99f, speedIncreaseInterval = 1.05f;  // 이동 속도 감소 인터벌
    [SerializeField] private float lightDamageInterval = 2f; // 빛에 의한 체력 감소 인터벌
    private float timeInLight = 0f; // 빛에 노출된 시간
    private float inFlashSoundVolume = 0.2f;
    private float inFlashSoundDelay = 1.5f;
    bool isSoundPlaying = false;
    private AudioSource flashlightAudioSource;
    [SerializeField] private AudioClip DebuffSound;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
        anim = GetComponent<EnemyAnimController>();
        dmg = GetComponent<ApplyDamage>();
        flashlightAudioSource = gameObject.AddComponent<AudioSource>();
        flashlightAudioSource.clip = DebuffSound;
        flashlightAudioSource.volume = inFlashSoundVolume;
        flashlightAudioSource.loop = true;
    }
    void Update()
    {
        if (enemy.CurrentState == State.Death)
            return;
        if (isInLight)
        {
            Debuff();
            timeInLight += Time.deltaTime;
            if (timeInLight >= lightDamageInterval)
            {
                timeInLight = 0f;
                if (!dmg.IsInvincible)
                    dmg.TakeDamage();
            }
        }
        else Restoration(enemy.CurrentState == State.Chasing ? enemy.MaxSpeed : enemy.BasicSpeed);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            isInLight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            isInLight = false;
            flashlightAudioSource.Pause();
            isSoundPlaying = false;
        }
    }
    void Debuff()
    {
        enemy.Speed = (enemy.Speed >= enemy.MinSpeed) ? enemy.Speed * speedDecreaseInterval : enemy.MinSpeed;
        if (!isSoundPlaying) StartCoroutine(InFlashLightSoundPlaying());
    }
    void Restoration(float upperLimit)
    {
        enemy.Speed = (enemy.Speed <= upperLimit) ? enemy.Speed * speedIncreaseInterval : upperLimit;
    }
    IEnumerator InFlashLightSoundPlaying()
    {
        isSoundPlaying = true;
        flashlightAudioSource.Play();

        yield return new WaitForSeconds(DebuffSound.length);
        isSoundPlaying = false;
    }
}
