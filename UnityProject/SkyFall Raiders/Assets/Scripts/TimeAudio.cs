using System.Collections;
using UnityEngine;
public class TimedAudio : MonoBehaviour
{
    public AudioSource src;

    public void PlayFor(float duration)
    {
        StartCoroutine(Co(duration));
    }

    private IEnumerator Co(float duration)
    {
        if (!src) yield break;
        src.Play();
        float t = 0f;
        while (t < duration && src) { t += Time.unscaledDeltaTime; yield return null; }
        if (src) src.Stop();
        Destroy(gameObject);
    }
    private void OnDestroy() { StopAllCoroutines(); }
}