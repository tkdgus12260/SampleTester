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

    [Header("판돈 증가량 Float 입력 ex) 1.5 = 0.5배 증가")]
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
            Debug.Log("유저 또는 방이 없습니다.");
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
                Debug.Log($"{ownerName} 방주인이 {userName} 입니다.");
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
                Debug.Log($"{ownerName} (방장) 이 {userName} 을(를) 이겼습니다.");
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
                Debug.Log($"{ownerName} (방장) 이 {userName} 에게 졌습니다.");
            }

            room.Stake = Mathf.FloorToInt(room.Stake * raseValue);
        }
        else
        {
            room.Owner = userName;
            currentUserIndex++;
            GameManager.Instance.ItemUI.InitializeGameUiItem(currentUserIndex, newRoomName, moveSteps, "X", "", userName, "");
            Debug.Log($"{userName} 이 {newRoomName} 방의 새로운 방장이 되었습니다.");
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
        roundText.text = $"싸이클 : {roundCount}";
    }

    public void UpdateTurnUI()
    {
        turnText.text = $"턴 : {turnCount}";
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
            raseText.text = $"판돈 배율 : {raseValue}배";
        }
        else
        {
            raseText.text = "판돈 배율을 소수로 입력해주세요.";
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
            Debug.Log("유저 이름과 값을 입력해주세요.");
        }


        if (roomNameIF.text != string.Empty && valueIF.text != string.Empty)
        {
            rooms[roomNameIF.text].Stake = int.Parse(valueIF.text);
        }
        else
        {
            Debug.Log("방 이름과 값을 입력해주세요.");
        }
    }
}
