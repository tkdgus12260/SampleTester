using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RoomManager : MonoBehaviour
{
    public List<string> RoomOrder = new List<string>();
    public Dictionary<string, RoomInfo> Rooms = new Dictionary<string, RoomInfo>();

    [SerializeField]
    private InputField roomNameIF = null;
    [SerializeField]
    private InputField roomStakeIF = null;


    public void AddRoom()
    {
        if (roomNameIF.text != string.Empty && roomStakeIF.text != string.Empty)
        {
            int num = StringToInt(roomStakeIF.text);
            if (num != 0)
            {
                string roomName = roomNameIF.text;

                if (Rooms.ContainsKey(roomName))
                {
                    Debug.Log("���� �̸��� ���� �̹� �ֽ��ϴ�.");
                    return;
                }
                RoomInfo room = new RoomInfo(roomNameIF.text,"", num, 0);
                GameManager.Instance.ItemUI.InitializeRoomItem(room.Name, "", room.Stake, 0);
                room.PropertyChanged += UpdateRoomItemUI;
                Rooms.Add(room.Name, room); 
                RoomOrder.Add(room.Name);
            }
            else
            {
                Debug.Log("���� �ݾ��� ���ڷ� �Է����ּ���.");
            }
        }
        else
        {
            Debug.Log("�� �̸�, ���� �ݾ��� �Է����ּ���.");
        }
    }
    private void UpdateRoomItemUI(RoomInfo roomInfo)
    {
        foreach (Transform child in GameManager.Instance.ItemUI.RoomContent.transform)
        {
            RoomItem roomItem = child.GetComponent<RoomItem>();
            if (roomItem != null && roomItem.RoomText.text == roomInfo.Name)
            {
                roomItem.InitItemUI(roomInfo.Name, roomInfo.Owner, roomInfo.Stake.ToString("N0"), roomInfo.GameCnt.ToString("N0"));
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
            Debug.Log($"{str}�� ������ ��ȯ�� �� �����ϴ�.");
            return 0;
        }
    }

    public void ResetRoomInfo()
    {
        RoomOrder.Clear();
        Rooms.Clear();
    }
}

public class RoomInfo
{
    public event Action<RoomInfo> PropertyChanged;

    public string Name; 
    private string owner;
    public string Owner
    {
        get { return owner; }
        set
        {
            owner = value;
            PropertyChanged?.Invoke(this);
        }
    }
    private int stake;
    public int Stake
    {
        get { return stake; }
        set
        {
            stake = value;
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
    public RoomInfo(string name,string owner, int stake, int gameCnt)
    {
        Name = name;
        Owner = owner;
        Stake = stake;
        GameCnt = gameCnt;
    }
}