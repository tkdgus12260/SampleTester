using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text RoomText;
    public Text OwnerText;
    public Text StakeText;
    public Text GameCountText;

    public void InitItemUI(string name, string owner, string stake, string gameCount)
    {
        RoomText.text = name;
        OwnerText.text = owner;
        StakeText.text = stake;
        GameCountText.text = gameCount;
    }
}
