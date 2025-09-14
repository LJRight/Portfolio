using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HazardLine : MonoBehaviour
{
    private Vector2 startPos, endPos;
    private float blinkPeriod, totalDuration, missileSpeed;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] float minAlpha = 0f, maxAlpha = 1f;
    [SerializeField] GameObject missilePrefab;
    public void Init(Vector2 startPos, Vector2 endPos, float blinkPeriod, float totalDuration, float speed)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.blinkPeriod = blinkPeriod;
        this.totalDuration = totalDuration;
        missileSpeed = speed;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        StartCoroutine(Blink());
    }
    private IEnumerator Blink()
    {
        float elapsed = 0f;
        Color start = lineRenderer.startColor;
        Color end = lineRenderer.endColor;
        AudioPlayer.Instance.PlayMissileWarningClip(totalDuration);
        while (elapsed < totalDuration)
        {
            float t01 = Mathf.PingPong(Time.time, blinkPeriod) / blinkPeriod;
            float a = Mathf.Lerp(minAlpha, maxAlpha, t01);
            start.a = a;
            end.a = a;
            lineRenderer.startColor = start;
            lineRenderer.endColor = end;
            elapsed += Time.deltaTime;
            yield return null;
        }
        lineRenderer.enabled = false;
        FireHazard();
    }
    private void FireHazard()
    {
        GameObject instance = Instantiate(missilePrefab, startPos, GetRotation());
        instance.GetComponent<Rigidbody2D>().linearVelocity = (endPos - startPos) * missileSpeed;
        Destroy(gameObject);
    }
    private Quaternion GetRotation()
    {
        Vector2 dir = (endPos - startPos).normalized;
        return Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
