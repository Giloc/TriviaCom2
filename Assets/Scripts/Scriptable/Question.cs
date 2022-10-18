using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Question", menuName = "Trivia/Question")]
public class Question : ScriptableObject
{
    [SerializeField] private string questionName;
    [SerializeField] private List<string> answers;
    [SerializeField] private string correctAnswer;


    public bool CompareAnswer(string answer){
        return answer == CorrectAnswer;
    }
    
    public List<string> Answers { get => answers; set => answers = value; }
    public string QuestionName { get => questionName; set => questionName = value; }
    public string CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }
}
