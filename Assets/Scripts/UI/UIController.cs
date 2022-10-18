using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public QuestionBank categorySelected;
    [SerializeField] private Text questionTitle;
    [SerializeField] private List<Text> answers;
    [SerializeField] private List<int> index;
    [SerializeField] private Question questionSelected;
    [SerializeField] private GameObject button, category, answerInterface;
    [SerializeField] private List<GameObject> names;
    [SerializeField] private List<Text> nameTexts;
    [SerializeField] private CategoryUI categoryUI;
    [SerializeField] private float connectionTime;
    private float connectionTimeControl;
    public string answerSelected;

    public bool listening;
    private bool questionIsSelected;
    private int players;

    private void Update() {
        if(listening){
            Listening();
        }
        GetPlayers();
    }

    private void Listening(){
        if(!questionIsSelected){
            WSClient._instance.SetQuestion();
            questionIsSelected = true;
        }
        if(categorySelected != null){
            listening = false;
            StartCoroutine("SetQuestion");
            return;
        }
    }

    public void StartListening(){
        connectionTimeControl = Time.time;
        while(!WSClient._instance.allReady){
            if((Time.time - connectionTimeControl) > connectionTime){
                Debug.Log("tiempo agotado");
                break;
            }
            continue;
        }
        listening = true;
        questionIsSelected = false;
        categoryUI.StartGetting();
    }

    public void SendReady(){
        WSClient._instance.SendReady();
    }

    public void CompareAnswer(Image answerButton){
        if(questionSelected.CompareAnswer(answerSelected)){
            Debug.Log("melo rey");
            WSClient._instance.SendAnswer(answerSelected, true);
            answerButton.color = Color.green;
        }
        else{
            Debug.Log("mera vuelta pa");
            WSClient._instance.SendAnswer(answerSelected, false);
            answerButton.color = Color.red;
        }
        categorySelected = null;
        StartCoroutine("BackToQuestion", answerButton);
    }

    private void GetPlayers(){
        if(WSClient._instance.PlayerNames.Count != players || WSClient._instance.namesChanged){
            for (int i = 0; i < WSClient._instance.PlayerNames.Count; i++)
            {
                names[i].SetActive(true);
                nameTexts[i].text = WSClient._instance.PlayerNames[i];
            }
            players = WSClient._instance.PlayerNames.Count;
            WSClient._instance.namesChanged = false;
        }

    }

    private IEnumerator SetQuestion(){
        yield return new WaitForSeconds(2f);
        button.SetActive(false);
        category.SetActive(false);
        answerInterface.SetActive(true);
        questionSelected = WSClient._instance.chosen;
        WSClient._instance.SetCorrectAnswer(questionSelected.CorrectAnswer);
        questionTitle.text = questionSelected.QuestionName;
        List<int> indexCopy = new List<int>(index);
        for (int i = 0; i < 4; i++){
            int j = indexCopy[Random.Range(0, indexCopy.Count - 1)];
            answers[j].text = questionSelected.Answers[i];
            indexCopy.Remove(j);
        }       
    }

    private IEnumerator BackToQuestion(Image answerButton){
        yield return new WaitForSeconds(2f);
        answerInterface.SetActive(false);
        button.SetActive(true);
        category.SetActive(true);
        answerButton.color = Color.white;
    }
}
