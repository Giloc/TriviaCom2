using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private List<QuestionBank> categories;
    [SerializeField] private UIController uiController;
    [SerializeField] private Text categoryText;
    [SerializeField] private float changeSpeed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    [SerializeField] private bool categoryGetted;
    private float settedTime;
    private float timeControl;
    private float speedControl;
    private int index;
    private int lastIndex;
    private bool categoryGetStarted;

    public Text CategoryText { get => categoryText; set => categoryText = value; }

    private void Start() {
        categoryGetted = true;
    }

    private void Update() {
        if(!categoryGetted){
            GetCategory();
        }
    }

    public void GetCategory(){
        if(!categoryGetStarted){
            timeControl = Time.time;
            settedTime = Random.Range(minTime, maxTime);
            categoryGetStarted = true;
        }
        if(Time.time - timeControl > settedTime){
            categoryGetted = true;
            categoryGetStarted = false;
            uiController.categorySelected = categories[lastIndex];
            CategoryText.text = WSClient._instance.questionBank.categoryName;
        }
        if(Time.time - speedControl > changeSpeed){
            CategoryText.text = categories[index].name;
            lastIndex = index;
            index = index == categories.Count - 1 ? 0 : index + 1;
            speedControl = Time.time;
        }
    }

    public void StartGetting(){
        categoryGetted = false;
    }
}
