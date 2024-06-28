using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    public Text UserText;
    public Text CashText;
    public Text GameCountText;

    public void InitItemUI(string name, string cash, string gameCount)
    {
        UserText.text = name;
        CashText.text = cash;
        GameCountText.text = gameCount;
    }
}
