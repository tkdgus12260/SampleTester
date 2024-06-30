using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).Name);
                    _instance = singleton.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    [HideInInspector]
    public UserManager UserManager;
    [HideInInspector]
    public RoomManager RoomManager;
    [HideInInspector]
    public ItemUI ItemUI;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UserManager = GetComponent<UserManager>();
        RoomManager = GetComponent<RoomManager>();
        ItemUI = GetComponent<ItemUI>();
    }
    private void Start()
    {
        SetResolution();
    }
    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;

        Screen.SetResolution(setWidth, setHeight, true);
    }
    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

