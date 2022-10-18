using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question Bank", menuName = "Trivia/Question Bank")]
public class QuestionBank : ScriptableObject
{
    [SerializeField] private List<Question> questions;
    public string categoryName;

    public List<Question> Questions { get => questions; set => questions = value; }

    public Question GetRandomQuestion(){
        int index = Random.Range(0, Questions.Count);
        return Questions[index];
    }
}
