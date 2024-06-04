using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOGameController : ScriptableObject
{
    public string Answer;
    public string[] AnswerAlternatives;
    public List<QuizQuestion> questions;

}
[System.Serializable]
public class QuizAnswer
{
    public string answerText;
    public bool isCorrect;
}
[System.Serializable]
public class QuizQuestion
{
    public string questionText;
    public List<List<QuizAnswer>> answerGroups;
}