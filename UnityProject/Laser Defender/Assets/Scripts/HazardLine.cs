using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HazardLine : MonoBehaviour
{
    private Vector2 startPos, endPos;
    private float blinkPeriod, totalDuration;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] float minAlpha = 0f, maxAlpha = 1f;
    [SerializeField] float testTimer = 0f, time = 10f;
    [SerializeField] Vector2 testSt, testEd;
    [SerializeField] float testPeriod, testDuration;
    [SerializeField] GameObject hazardPrefab;
    bool testbool = true;
    void Update()
    {
        testTimer += Time.deltaTime;
        if (testTimer >= time && testbool)
        {
            testbool = false;
            Init(testSt, testEd, testPeriod, testDuration);
        }
    }
    public void Init(Vector2 startPos, Vector2 endPos, float blinkPeriod, float totalDuration)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.blinkPeriod = blinkPeriod;
        this.totalDuration = totalDuration;
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

    }
}
