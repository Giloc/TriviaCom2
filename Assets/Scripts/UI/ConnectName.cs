using UnityEngine;
using UnityEngine.UI;

public class ConnectName : MonoBehaviour
{
    [SerializeField] private InputField playerInputName;
    private string playerName;
    public void Connect(){
        playerName = playerInputName.text;
        WSClient._instance.Connect(playerName);
    }
}
