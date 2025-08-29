using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Image timerImage;
    [SerializeField] float timeoutPeriod = 10.0f;
    [SerializeField] float timerValue;
    private bool isTimerEnable = true;
    [SerializeField] Quiz quiz;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isTimerEnable)
        {
            timerValue += Time.deltaTime;
            if (timerValue >= timeoutPeriod)
                quiz.Timeout();
            else
                timerImage.fillAmount = 1 - timerValue / timeoutPeriod;
        }
    }
    public void ResetTimer()
    {
        timerValue = 0;
        timerImage.fillAmount = 1;
    }
    public void StartTimer()
    {
        audio.Play();
        isTimerEnable = true;
    }
    public void StopTimer()
    {
        audio.Stop();
        isTimerEnable = false;
    }
}
