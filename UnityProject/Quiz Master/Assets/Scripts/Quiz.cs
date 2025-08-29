using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using Unity.VisualScripting;
public class Quiz : MonoBehaviour
{
    [SerializeField] QuestionSO[] questions;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] float answerCheckDelay = 1.5f;
    [SerializeField] int quizNumber;
    int currentQuizIndex = 0;
    [SerializeField] int correctAnswerIndex;
    [SerializeField] Timer timer;
    [Header("Button Sprites")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Answer SFX")]
    [SerializeField] AudioClip correctAnsSFX;
    [SerializeField] AudioClip incorrectAnsSFX;

    AudioSource audio;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    [SerializeField] Score scoreCanvas;
    Canvas canvas;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 1;
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        progressBar.maxValue = questions.Length;
        progressBar.value = 0;
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        questionText.text = questions[currentQuizIndex].Question;
        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuizIndex].GetAnswer(i);
        correctAnswerIndex = questions[currentQuizIndex].AnswerIndex;
        timer.StartTimer();
    }

    void GetNextQuestion()
    {
        currentQuizIndex++;
        progressBar.value++;
        timer.ResetTimer();
        SetDefaultButtonSprites();
        if (progressBar.value == questions.Length)
        {
            DisplayScoreCanvas();
            return;
        }
        SetButtonState(true);
        DisplayQuestion();
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponent<Button>().interactable = state;
    }

    public void Timeout()
    {
        SetButtonState(false);
        timer.StopTimer();
        DisplayAnswerButton();
        questionText.text = "Time OUT!!! Answer was...\n" + questions[currentQuizIndex].GetAnswer(correctAnswerIndex);
        scoreKeeper.ChangeScore(false);
        audio.PlayOneShot(incorrectAnsSFX);
        StartCoroutine(GetNextQuestionWithDelay());
    }

    public void OnAnswerSelected(int ans)
    {
        DisplayAnswerButton();
        SetButtonState(false);
        timer.StopTimer();

        if (ans == correctAnswerIndex)
        {
            questionText.text = "Correct !!";
            scoreKeeper.ChangeScore(true);
            audio.PlayOneShot(correctAnsSFX);
        }
        else
        {
            questionText.text = "Sorry, the correct answer was...\n" + questions[currentQuizIndex].GetAnswer(correctAnswerIndex);
            scoreKeeper.ChangeScore(false);
            audio.PlayOneShot(incorrectAnsSFX);
        }
        StartCoroutine(GetNextQuestionWithDelay());
    }

    IEnumerator GetNextQuestionWithDelay()
    {
        yield return new WaitForSeconds(answerCheckDelay);
        GetNextQuestion();
    }
    void SetDefaultButtonSprites()
    {
        answerButtons[correctAnswerIndex].GetComponent<Image>().sprite = defaultAnswerSprite;
    }
    void DisplayAnswerButton()
    {
        answerButtons[correctAnswerIndex].GetComponent<Image>().sprite = correctAnswerSprite;
    }

    void DisplayScoreCanvas()
    {
        canvas.sortingOrder = -1; 
        scoreCanvas.ActiveCanvas();
    }
}
