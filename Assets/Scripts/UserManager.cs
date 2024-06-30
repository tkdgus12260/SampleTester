using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UserManager : MonoBehaviour
{
    public List<string> UserOrder = new List<string>();
    public Dictionary<string, UserInfo> Users = new Dictionary<string, UserInfo>();

    [SerializeField]
    private InputField userNameIF = null;
    [SerializeField]
    private InputField userCashIF = null;


    public void AddUserList()
    {
        //UserList
    }

    public void AddUser()
    {
        if (userNameIF.text != string.Empty && userCashIF.text != string.Empty)
        {
            int num = StringToInt(userCashIF.text);
            if (num != 0)
            {
                string userName = userNameIF.text;

                if (Users.ContainsKey(userName))
                {
                    Debug.Log("같은 이름의 유저가 이미 있습니다.");
                    return;
                }
                UserInfo user = new UserInfo(userNameIF.text, num, 0, 0);
                GameManager.Instance.ItemUI.InitializeUserItem(user.Name, user.Cash, 0, 0);
                user.PropertyChanged += UpdateUserItemUI;
                Users.Add(user.Name, user);
                UserOrder.Add(user.Name);
            }
            else
            {
                Debug.Log("보유 금액을 숫자로 입력해주세요.");
            }
        }
        else
        {
            Debug.Log("유저 이름, 보유 금액을 입력해주세요.");
        }
    }
    private void UpdateUserItemUI(UserInfo userInfo)
    {
        foreach (Transform child in GameManager.Instance.ItemUI.UserContent.transform)
        {
            UserItem userItem = child.GetComponent<UserItem>();
            if (userItem != null && userItem.UserText.text == userInfo.Name)
            {
                userItem.InitItemUI(userInfo.Name, userInfo.Cash.ToString("N0"), userInfo.GameCnt.ToString("N0"), userInfo.CurrentRoom.ToString("N0"));
                break;
            }
        }
    }
    private int StringToInt(string str)
    {
        try
        {
            return int.Parse(str);
        }
        catch (FormatException)
        {
            Debug.Log($"{str}을 정수로 변환할 수 없습니다.");
            return 0;
        }
    }

    public void ResetUserInfo()
    {
        UserOrder.Clear();
        Users.Clear();
    }

}

public class UserInfo
{
    public event Action<UserInfo> PropertyChanged;

    public string Name;
    private int cash;
    public int Cash
    {
        get { return cash; }
        set
        {
            cash = value;
            PropertyChanged?.Invoke(this);
        }
    }
    private int gameCnt;
    public int GameCnt
    {
        get { return gameCnt; }
        set
        {
            gameCnt = value;
            PropertyChanged?.Invoke(this);
        }
    }
    private int currentRoom;

    public int CurrentRoom
    {
        get { return currentRoom; }
        set
        {
            currentRoom = value;
            PropertyChanged?.Invoke(this);
        }
    }

    public UserInfo(string name, int cash, int gameCnt, int currentRoom)
    {
        Name = name;
        Cash = cash;
        GameCnt = gameCnt;
        CurrentRoom = currentRoom;
    }
}