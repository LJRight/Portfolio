using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField][Range(0f, 1f)] float shootingVolume = 1f;
    [Header("Damage")]
    [SerializeField] AudioClip playerDamagedClip;
    [SerializeField][Range(0f, 1f)] float playerDamagedVolume = 1f;
    [SerializeField] AudioClip enemyDamagedClip;
    [SerializeField][Range(0f, 1f)] float enemyDamagedVolume = 1f;
    [SerializeField] float pitch = 1.5f;
    [Header("ItemSFX")]
    [SerializeField] AudioClip coinClip;
    [SerializeField][Range(0f, 1f)] float coinVolume = 1f;
    [SerializeField] AudioClip powerUpClip;
    [SerializeField][Range(0f, 1f)] float powerUpVolume = 1f;
    [SerializeField] AudioClip shopItemClip;
    [SerializeField][Range(0f, 1f)] float shopItemVolume = 1f;

    [Header("UI")]
    [SerializeField] AudioClip buttonSelectClip;
    [SerializeField][Range(0f, 1f)] float buttonSelectVolume = 1f;

    [Header("BGM")]
    [SerializeField] AudioSource mainBGMSource;
    [SerializeField] List<AudioClip> BGMList;
    [SerializeField] float BGMVolume = 0.65f;
    float bgmSliderValue = 1.0f, sfxSliderValue = 1.0f;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void PlayClip(AudioClip source, float volume)
    {
        if (source != null)
            AudioSource.PlayClipAtPoint(source,
                                        Camera.main.transform.position,
                                        volume * sfxSliderValue);
    }
    public void PlayShootingClip()
    {
        PlayClip(shootingClip, shootingVolume);
    }
    public void PlayPlayerDamagedClip()
    {
        PlayClip(playerDamagedClip, playerDamagedVolume);
    }
    public void PlayCoinClip()
    {
        PlayClip(coinClip, coinVolume);
    }
    public void PlayPowerUpClip()
    {
        PlayClipWithPitch(powerUpClip, powerUpVolume, pitch);
    }
    public void PlayShopItemClip()
    {
        PlayClip(shopItemClip, shopItemVolume);
    }
    public void PlayEnemyDamagedClip()
    {
        PlayClipWithPitch(enemyDamagedClip, enemyDamagedVolume, pitch);
    }
    private void PlayClipWithPitch(AudioClip clip, float volume, float pitch)
    {
        if (clip == null) return;
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = Camera.main.transform.position;
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * sfxSliderValue;
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(tempGO, clip.length / pitch);
    }
    public void PlayButtonSelectClip()
    {
        PlayClip(buttonSelectClip, buttonSelectVolume);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainBGMSource.clip = BGMList[scene.buildIndex];
        mainBGMSource.volume = BGMVolume * bgmSliderValue;
        mainBGMSource.Play();
    }
    public void SetBGMVolume(float ratio)
    {
        bgmSliderValue = ratio;
        mainBGMSource.volume = BGMVolume * bgmSliderValue;
    }
    public void SetSFXVolume(float ratio)
    {
        sfxSliderValue = ratio;
    }
}
