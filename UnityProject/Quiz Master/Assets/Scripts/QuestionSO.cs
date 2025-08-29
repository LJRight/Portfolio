using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    public string Question { get { return question; } }
    [SerializeField] string[] answersButton = new string[4];
    [SerializeField] int answerIndex;
    public int AnswerIndex { get { return answerIndex; } }

    public string GetAnswer(int index)
    {
        return answersButton[index];
    }
}