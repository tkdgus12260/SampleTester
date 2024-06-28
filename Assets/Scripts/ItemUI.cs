using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [Header("------UserInfo------")]
    public GameObject UserContent;
    [SerializeField]
    private GameObject userPrefab;

    [Header("------RoomInfo------")]
    public GameObject RoomContent;
    [SerializeField]
    private GameObject roomPrefab;

    [Header("------GameUiInfo------")]
    public GameObject GameUiContent;
    [SerializeField]
    private GameObject GameUiPrefab;

    private List<GameObject> userItemList = new List<GameObject>();
    private List<GameObject> roomItemList = new List<GameObject>();
    private List<GameObject> gameUiItemList = new List<GameObject>();

    public void InitializeUserItem(string name, int cash, int gameCount)
    {
        GameObject newUserItem = Instantiate(userPrefab, UserContent.transform);
        userItemList.Add(newUserItem);

        UserItem userItem = newUserItem.GetComponent<UserItem>();

        userItem.InitItemUI(name, cash.ToString("N0"), gameCount.ToString("N0"));
    }

    public void InitializeRoomItem(string name,string owner, int stake, int gameCount)
    {
        GameObject newRoomItem = Instantiate(roomPrefab, RoomContent.transform);
        roomItemList.Add(newRoomItem);

        RoomItem roomItem = newRoomItem.GetComponent<RoomItem>();

        roomItem.InitItemUI(name, owner, stake.ToString("N0"), gameCount.ToString("N0"));
    }

    public void InitializeGameUiItem(int turn, int rouletteNum, string hasPlayed, string owner, string player, string winner)
    {
        GameObject newGameUiItem = Instantiate(GameUiPrefab, GameUiContent.transform);
        gameUiItemList.Add(newGameUiItem);

        GameUiItem gameUiItem = newGameUiItem.GetComponent<GameUiItem>();

        gameUiItem.InitItemUI(turn.ToString("N0"), rouletteNum.ToString("N0"), hasPlayed, owner, player, winner);
    }

    public void InitializeNullGameUiItem()
    {
        GameObject newGameUiItem = Instantiate(GameUiPrefab, GameUiContent.transform);
        gameUiItemList.Add(newGameUiItem);

        Image image = newGameUiItem.GetComponent<Image>();
        image.color = Color.red;
    }

    public void AllDestoryItemObject()
    {
        DestroyAllItemsInStack(userItemList);
        DestroyAllItemsInStack(roomItemList);
        DestroyAllItemsInStack(gameUiItemList);
    }
    private void DestroyAllItemsInStack(List<GameObject> list)
    {
        foreach(GameObject item in list)
        {
            Destroy(item);
        }
    }
}
