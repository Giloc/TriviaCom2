using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private Text answer;
    [SerializeField] private Image buttonImage;

    public void SetAnswer(){
        uiController.answerSelected = answer.text;
        uiController.CompareAnswer(buttonImage);
    }
}
