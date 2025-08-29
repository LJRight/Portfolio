using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Animator crossFadeAnimator;
    bool isButtonClicked;

    public static GameOver instance;

    void Awake()
    {
        instance = this;
        crossFadeAnimator.GetComponent<Animator>();
        DontDestroyOnLoad(instance);
    }

    public void RestartClick()
    {
        isButtonClicked = false;
        GameManager.instance.GameOverImage.SetActive(false);
        // SoundManager.instance.gameObject.SetActive(false);
        LoadRestartScene();
    }

    public void MainMenuClick()
    {
        isButtonClicked = false;
        GameManager.instance.GameOverImage.SetActive(false);
        SoundManager.instance.gameObject.SetActive(false);
        LoadMainScene();
    }

    public void LoadRestartScene()
    {
        StartCoroutine(LoadScene(GameManager.instance.stage));
        Time.timeScale = 1;
        GameManager.instance.hp = GameManager.instance.maxHp;
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadScene(6));
        Time.timeScale = 1;
        GameManager.instance.hp = GameManager.instance.maxHp;
    }

    IEnumerator LoadScene(int index)
    {
        crossFadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);

        crossFadeAnimator.SetTrigger("End");
        SceneManager.LoadScene(index);
        GameManager.instance.isDead = false;
        GameManager.instance.isCliff = false;
        UI_Life.instance.ResetUILife();
        isButtonClicked = true;
    }
}
