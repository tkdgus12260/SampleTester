using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiItem : MonoBehaviour
{
    public Text TurnText;
    public Text RouletteText;
    public Text HasPlayedText;
    public Text OwnerText;
    public Text PlayerText;
    public Text WinnerText;

    public void InitItemUI(string turn, string rouletteNum, string hasPlayed, string owner, string Player, string winner) 
    {
        TurnText.text = turn;
        RouletteText.text = rouletteNum;
        HasPlayedText.text = hasPlayed;
        OwnerText.text = owner;
        PlayerText.text = Player;
        WinnerText.text = winner;
    }
}
