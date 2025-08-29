using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLightWeaknesses : MonoBehaviour
{
    private List<Behaviour> componentsToDisable = new List<Behaviour>();
    [SerializeField] private Collider2D collider;
    private Rigidbody2D rigid;
    private SpriteRenderer renderer;
    [SerializeField] private bool isInBlueLight = false;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 1.0f;

    void Awake()
    {
        foreach (var component in GetComponents<Behaviour>())
        {
            if (component != this)
                componentsToDisable.Add(component);
        }
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public void OnChildTriggerEnter2D()
    {
        DisableComponents();
    }
    public void OnChildTriggerExit2D()
    {
        EnableComponents();
    }

    void DisableComponents()
    {
        foreach (var component in componentsToDisable)
        {
            component.enabled = false;
        }
        collider.enabled = false;
        if (rigid != null)
        {
            rigid.simulated = false;
        }
        StartCoroutine(FadeOutSprite());
    }

    void EnableComponents()
    {
        foreach (var component in componentsToDisable)
        {
            component.enabled = true;
        }
        collider.enabled = true;
        if (rigid != null)
        {
            rigid.simulated = true;
        }
        StartCoroutine(FadeInSprite());
    }

    IEnumerator FadeOutSprite()
    {
        if (renderer != null)
        {
            Color color = renderer.color;
            float startAlpha = color.a;

            for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
            {
                float blend = t / fadeOutDuration;
                color.a = Mathf.Lerp(startAlpha, 0, blend);
                renderer.color = color;
                yield return null;
            }

            color.a = 0;
            renderer.color = color;
            renderer.enabled = false;
        }
    }

    IEnumerator FadeInSprite()
    {
        if (renderer != null)
        {
            renderer.enabled = true;
            Color color = renderer.color;
            float startAlpha = color.a;

            for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
            {
                float blend = t / fadeInDuration;
                color.a = Mathf.Lerp(startAlpha, 1, blend);
                renderer.color = color;
                yield return null;
            }

            color.a = 1;
            renderer.color = color;
        }
    }
}
