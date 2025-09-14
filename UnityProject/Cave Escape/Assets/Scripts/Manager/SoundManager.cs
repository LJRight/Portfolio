using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum BGM
    {
        Thema,
    }

    public enum SFX
    {
        Jump,
        Run,
        Dead,
        Live,
        LightOnOff,
        BoxOpen,
        PlayerHit,
        DoorOpen,
    }

    [SerializeField]
    AudioClip[] BGMaudioClips;

    [SerializeField]
    AudioClip[] SFXaudioClips;

    [SerializeField]
    public AudioSource BGMaudioSource;

    [SerializeField]
    public AudioSource SFXaudioSource;

    void Awake()
    {
        instance = this;
        // BGMaudioSource.PlayOneShot(BGMaudioClips[(int)BGM.Thema]);
        BGMaudioSource.clip = BGMaudioClips[(int)BGM.Thema];
        BGMaudioSource.loop = true;
        BGMaudioSource.Play();
        DontDestroyOnLoad(instance);
    }

    public void PlaySFX(SFX sfx, float Volume)
    {
        SFXaudioSource.PlayOneShot(SFXaudioClips[(int)sfx], Volume);
    }
}
