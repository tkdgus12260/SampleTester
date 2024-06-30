using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    public Text UserText;
    public Text CashText;
    public Text GameCountText;
    public Text CurrentRoom;

    public void InitItemUI(string name, string cash, string gameCount, string currentRoom)
    {
        UserText.text = name;
        CashText.text = cash;
        GameCountText.text = gameCount;
        CurrentRoom.text = currentRoom;
    }
}
