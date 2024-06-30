using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnGameManager : MonoBehaviour
{
    private List<string> userOrder = new List<string>();
    private List<string> roomOrder = new List<string>();
    private Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();
    private Dictionary<string, RoomInfo> rooms = new Dictionary<string, RoomInfo>();

    private int currentUserIndex = 0;
    private int roundCount = 1;
    private int turnCount = 0;

    [SerializeField]
    private Text roundText;
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private InputField userNameIF;
    [SerializeField]
    private InputField roomNameIF;
    [SerializeField]
    private InputField valueIF;

    [Header("�ǵ� ������ Float �Է� ex) 1.5 = 0.5�� ����")]
    [SerializeField]
    private InputField raseIF;
    [SerializeField]
    private Text raseText;
    private float raseValue = 1.0f;

    private void Start()
    {
        userOrder = GameManager.Instance.UserManager.UserOrder;
        roomOrder = GameManager.Instance.RoomManager.RoomOrder;
        users = GameManager.Instance.UserManager.Users;
        rooms = GameManager.Instance.RoomManager.Rooms;
    }

    public void GameStart()
    {

        if (userOrder.Count == 0 || roomOrder.Count == 0)
        {
            Debug.Log("���� �Ǵ� ���� �����ϴ�.");
            return;
        }

        List<string> usersCopy = userOrder.ToList();

        foreach (var roomName in roomOrder)
        {
            if (usersCopy.Count <= 0) break;

            int num = Random.Range(0, usersCopy.Count);
            string randomUserName = usersCopy[num];

            if (rooms.ContainsKey(roomName) && users.ContainsKey(randomUserName))
            {
                rooms[roomName].Owner = randomUserName;
                usersCopy.RemoveAt(num);
            }
        }

        UpdateRoundUI();
    }
    public void MoveUsersAndBattle()
    {
        if (userOrder.Count <= 1) return;

        turnCount++;
        UpdateTurnUI();

        if (currentUserIndex >= userOrder.Count)
        {
            currentUserIndex = 0;
            GameManager.Instance.ItemUI.InitializeNullGameUiItem();

            roundCount++;
            UpdateRoundUI();
        }

        string userName = userOrder[currentUserIndex];
        UserInfo user = users[userName];

        int moveSteps = Random.Range(1, roomOrder.Count + 1);
        int currentRoomIndex = user.CurrentRoom;

        currentRoomIndex = (currentRoomIndex + moveSteps - 1) % roomOrder.Count;
        user.CurrentRoom = currentRoomIndex + 1;
        Debug.Log("@@@@@@@@@@@@  1  : " + currentRoomIndex);
        //else
        //{
        //    currentRoomIndex = (currentRoomIndex + moveSteps) % roomOrder.Count;
        //    user.CurrentRoom = currentRoomIndex;
        //}

        string newRoomName = roomOrder[currentRoomIndex];
        RoomInfo room = rooms[newRoomName];

        Debug.Log("@@@@@@@@@@@@  2  : " + currentRoomIndex);
        if (room.Owner != string.Empty)
        {
            string ownerName = room.Owner;

            if (user.Name == ownerName)
            {
                Debug.Log($"{ownerName} �������� {userName} �Դϴ�.");
                currentUserIndex++;
                GameManager.Instance.ItemUI.InitializeGameUiItem(currentUserIndex, newRoomName, moveSteps, "X", ownerName, userName, "");
                return;
            }

            int battleResult = Random.Range(0, 2);

            if (battleResult == 0)
            {
                if (user.Cash <= room.Stake)
                {

                    users[ownerName].Cash += user.Cash;
                    user.Cash = 0;
                    userOrder.Remove(user.Name);
                }
                else
                {
                    user.Cash -= room.Stake;
                    users[ownerName].Cash += room.Stake;
                    if(user.Cash == 0)
                    {
                        userOrder.Remove(user.Name);
                    }
                }
                room.GameCnt++;

                user.GameCnt++;
                users[ownerName].GameCnt++;
                currentUserIndex++;
                GameManager.Instance.ItemUI.InitializeGameUiItem(currentUserIndex, newRoomName, moveSteps, "O", ownerName, userName, ownerName);
                Debug.Log($"{ownerName} (����) �� {userName} ��(��) �̰���ϴ�.");
            }
            else
            {
                if (users[ownerName].Cash <= room.Stake)
                {
                    user.Cash += users[ownerName].Cash;
                    users[ownerName].Cash = 0;
                    userOrder.Remove(users[ownerName].Name);
                }
                else
                {
                    user.Cash += room.Stake;
                    users[ownerName].Cash -= room.Stake;
                    if (users[ownerName].Cash == 0)
                    {
                        userOrder.Remove(user.Name);
                    }
                }
                room.Owner = userName;
                room.GameCnt++;

                user.GameCnt++;
                users[ownerName].GameCnt++;
                currentUserIndex++;
                GameManager.Instance.ItemUI.InitializeGameUiItem(currentUserIndex, newRoomName, moveSteps, "O", ownerName, userName, userName);
                Debug.Log($"{ownerName} (����) �� {userName} ���� �����ϴ�.");
            }

            room.Stake = Mathf.FloorToInt(room.Stake * raseValue);
        }
        else
        {
            room.Owner = userName;
            currentUserIndex++;
            GameManager.Instance.ItemUI.InitializeGameUiItem(currentUserIndex, newRoomName, moveSteps, "X", "", userName, "");
            Debug.Log($"{userName} �� {newRoomName} ���� ���ο� ������ �Ǿ����ϴ�.");
        }
    }

    public void RoundSkipped()
    {
        for(int i = 0; i < userOrder.Count; i++)
        {
            MoveUsersAndBattle();
        }
    }

    public void UpdateRoundUI()
    {
        roundText.text = $"����Ŭ : {roundCount}";
    }

    public void UpdateTurnUI()
    {
        turnText.text = $"�� : {turnCount}";
    }

    public void ResetGame()
    {
        currentUserIndex = 0;
        roundCount = 1;
        turnCount = 0;
        UpdateRoundUI();
        UpdateTurnUI();
        GameManager.Instance.ItemUI.AllDestoryItemObject();
        GameManager.Instance.RoomManager.ResetRoomInfo();
        GameManager.Instance.UserManager.ResetUserInfo();
    }

    public void RaseValue()
    {
        if (raseIF.text != string.Empty)
        {
            raseValue = float.Parse(raseIF.text);
            raseText.text = $"�ǵ� ���� : {raseValue}��";
        }
        else
        {
            raseText.text = "�ǵ� ������ �Ҽ��� �Է����ּ���.";
        }
    }

    public void ChangeInfoValue()
    {
        if (userNameIF.text != string.Empty && valueIF.text != string.Empty)
        {
            users[userNameIF.text].Cash = int.Parse(valueIF.text);
        }
        else
        {
            Debug.Log("���� �̸��� ���� �Է����ּ���.");
        }


        if (roomNameIF.text != string.Empty && valueIF.text != string.Empty)
        {
            rooms[roomNameIF.text].Stake = int.Parse(valueIF.text);
        }
        else
        {
            Debug.Log("�� �̸��� ���� �Է����ּ���.");
        }
    }
}
