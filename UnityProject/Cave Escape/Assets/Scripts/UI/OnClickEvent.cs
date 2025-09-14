using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickEvent : MonoBehaviour
{
    public Animator crossFadeAnimator;

    void Awake()
    {
        crossFadeAnimator.GetComponent<Animator>();
    }

    public void ClickStart()
    {
        LoadNextScene();
    }

    public void ClickQuit()
    {
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene("Stage1"));
    }

    IEnumerator LoadScene(string stage)
    {
        crossFadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);

        crossFadeAnimator.SetTrigger("End");
        SceneManager.LoadScene(stage);
    }
}
