using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerShooterFadeIn : MonoBehaviour
{
    SpriteRenderer myRenderer;
    [SerializeField] float fadeDuration = 0.5f;
    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        float elapsed = 0f;
        Color c = myRenderer.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            c.a = alpha;
            myRenderer.color = c;
            yield return null;
        }
        c.a = 1f;
        myRenderer.color = c;
    }
}
