using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDoor : MonoBehaviour
{
    public GameObject canvas;
    Animator crossFadeAnimator;

    void Awake()
    {
        canvas = GameObject.Find("UI");
        GameObject crossFadeTransform = canvas.transform.Find("CrossFade")?.gameObject;
        if (crossFadeTransform != null)
        {
            crossFadeAnimator = crossFadeTransform.GetComponent<Animator>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //crossFadeAnimator.enabled = true;
            if (GameManager.instance.hasKey)
            {
                GameManager.instance.stage++;
                SoundManager.instance.PlaySFX(SoundManager.SFX.DoorOpen, 0.6f);
                LoadNextScene();

                GameManager.instance.hasKey = false;
            }
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(GameManager.instance.stage));

        if (GameManager.instance.GameOverUI != null)
        {
            GameManager.instance.GameOverImage = GameManager.instance.GameOverUI.transform.Find("GameOverImage")?.gameObject;
        }
    }

    IEnumerator LoadScene(int stage)
    {
        crossFadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);

        crossFadeAnimator.SetTrigger("End");
        if (stage == 4) stage = 6;
        SceneManager.LoadScene(stage);
    }
}
