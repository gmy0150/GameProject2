using System;
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
    public TutorialManager tutorialManager;
    private static GameManager _instance;
    public AnimationManage animation;
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
        animation = new AnimationManage();
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
        SavePos();

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
        SavePos();

    }
    public AilionAI Ailion;
    public void OnAilionDiaglogueEnd()
    {
        //ailion 소환
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
            enemy.SliderActive(false);
        }
        Ailion.gameObject.SetActive(true);
        Ailion.ChaseStart(player);
    }
    bool AIlionON = false;
    [SerializeField] GameObject wallexit;
    public void ONAilionPic()
    {
        wallexit.SetActive(false);
        AIlionON = true;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Third_Scene");
        if (prefab != null)
        {
            Instantiate(prefab);
        }
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
        if (!x)
        {
            animation.TransAnim(player.animator, "Walking", false);
            animation.TransAnim(player.animator, "Running", false);
        }
        SavePos();

    }
    void Start()
    {
        ActPlay(false);
        player.Move(tutoPos, false);
    }
    bool isMainGame;
    public void MainGameStart()
    {
        SavePos();
        gamestartPos.y = player.transform.position.y;
        player.transform.position = gamestartPos;
        player.transform.rotation = Quaternion.identity;
        ActPlay(false);
        isMainGame = true;
    }
    float gameTimer;
    void Update()
    {
        if (isMainGame)
        {
            gameTimer += Time.deltaTime;
            if (gameTimer > 1.5f)
            {
                player.Move(gameMovePos, true);
                isMainGame = false;
            }
        }
        if( Input.GetKeyDown(KeyCode.R))
        {
            OnAilionDiaglogueEnd();
        }
    }

    public void GameOver(Enemy guard)
    {
        StartCoroutine(GameRestart(guard));
    }
    Vector3 savePos;
    public void SavePos()
    {
        savePos = player.transform.position;
    }
    IEnumerator GameRestart(Enemy guard)
    {
        yield return new WaitForSeconds(1f);
        guard.InitNode();
        guard.transform.position = guard.startPos;
        player.transform.position = savePos;
        player.transform.rotation = Quaternion.identity;
        player.restart(); 
    }
}
