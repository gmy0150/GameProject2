using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject computer, Btn;

    public List<GameObject> AllPC = new List<GameObject>();
    bool OnComputer, DoorBtn = false;
    public Player player;
    public Vector3 tutoPos;
    public Vector3 gamestartPos;
    public Vector3 gameMovePos;
    private static GameManager _instance;
    public TutorialManager tutorialManager;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            }
            return _instance;
        }
    }
    public int PcCount()
    {
        return AllPC.Count;
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        tutorialManager = new TutorialManager();
        DontDestroyOnLoad(gameObject);
    }
    public bool AbleComputer()
    {
        return OnComputer;
    }
    public void OnComputerActive()
    {
        Debug.Log("컴퓨터 켜짐");
        OnComputer = true;
        computer.layer = 12;
        if (!computer.GetComponent<ComputerBtn>())
            computer.AddComponent<ComputerBtn>();
    }
    public bool AbleButton()
    {
        return DoorBtn;
    }
    public void OnDoorActive()
    {
        DoorBtn = true;
        Btn.layer = 12;
        if (!Btn.GetComponent<DoorBtn>())
            Btn.AddComponent<DoorBtn>();
    }
    bool AIlionON = false;
    [SerializeField] GameObject wallexit;
    public void OnAilion()
    {
        wallexit.SetActive(false);
        AIlionON = true;
    }
    public bool AbleExit()
    {
        return AIlionON;
    }
    bool canPlay = true;
    public bool CanPlayerMove()
    {
        return canPlay;
    }
    public void ActPlay(bool x)
    {
        canPlay = x;
    }
    void Start()
    {
        ActPlay(false);
        player.Move(tutoPos,false);
    }
    bool isMainGame;
    public void MainGameStart()
    {
        gamestartPos.y = player.transform.position.y;
        player.transform.position = gamestartPos;
        player.transform.rotation = Quaternion.identity;
        ActPlay(false);
        isMainGame = true;
    }
    float gameTimer;
    void Update()
    {
        if(isMainGame){
            gameTimer += Time.deltaTime;
            if(gameTimer > 1.5f){
                player.Move(gameMovePos,true);
                isMainGame = false;
            }
        }
    }
}
