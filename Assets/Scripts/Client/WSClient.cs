using WebSocketSharp;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WSClient : MonoBehaviour
{
    public static WSClient _instance;
    [SerializeField] private List<QuestionBank> questionBanks;
    [SerializeField] private List<string> playerNames;
    WebSocket ws;
    public bool habemus;
    public bool allReady;
    public Question chosen;
    public QuestionBank questionBank;
    private int playerId;
    public bool namesChanged;

    private void Awake() {
        if(_instance != null && _instance != this){
            DestroyImmediate(this.gameObject);
        }
        else if (_instance is null){
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.OnOpen += Open;
        ws.OnMessage += Message;
    }

    private void Update()
    {
        if (ws == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("1");
        }

        else if(Input.GetKeyDown(KeyCode.A)){
            ws.Send("score");
        }
    }

    public void Connect(string name){
        ws.Connect();
        ws.Send("connect " + name);
    }

    private void Open(System.Object sender, EventArgs e){
        Debug.Log("El jugador se ha conectado");
        ws.Send("bancos " + questionBanks.Count);
        for (int i = 0; i < questionBanks.Count; i++){
            ws.Send("bank " + questionBanks[i].Questions.Count);
        }
        SceneManager.LoadScene(1);
    }

    private void Message(System.Object sender, MessageEventArgs e){
        Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        if(e.Data.Contains("Question")){
            int banco = int.Parse(e.Data.Split(' ')[2]);
            int pregunta = int.Parse(e.Data.Split(' ')[1]);
            chosen = questionBanks[banco].Questions[pregunta];
            questionBank = questionBanks[banco];
            Debug.Log(questionBank.categoryName);
            habemus = true;
        }
        else if(e.Data.Contains("start")){
            allReady = true;
        }
        else if(e.Data.Contains("player")){
            string name = e.Data.Split(new string[] {"player "}, StringSplitOptions.None)[1];
            if(!playerNames.Contains(name)){
                playerNames.Add(name);
            }
        }
        else if(e.Data.Contains("puntaje ")){
            Debug.Log("a cambiar");
            string[] index = e.Data.Split(new string[] { " indice " }, StringSplitOptions.None);
            Debug.Log(index);
            int indexInt = int.Parse(index[1]);
            Debug.Log(indexInt);
            string name = index[0].Split(new string[] {"puntaje "}, StringSplitOptions.None)[1];
            playerNames[indexInt] = name;
            Debug.Log(name);
            if(indexInt == playerNames.Count -1){
                namesChanged = true;
            }
        }
        else if(e.Data.Contains("se ha conectado! con el id")){
            string[] cadena = e.Data.Split(' ');
            playerId = int.Parse(cadena[cadena.Length - 1]);
        }
       
    }

    public void SetCorrectAnswer(string answer){
        ws.Send("answer " + answer);
    }

    public void SetQuestion(){
        ws.Send("request");
    }

    public void SendAnswer(string answer, bool correct){
        if(correct){
            ws.Send("selected " + playerId);
        }
        else{
            ws.Send("wrong " + playerId);
        }
    }

    public void SendReady(){
        ws.Send("ready");
    }
    public List<string> PlayerNames { get => playerNames; set => playerNames = value; }
}
