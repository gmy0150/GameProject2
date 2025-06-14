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
    Enemy[] enemies;
    FadeInScene fadeInScene;
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
        fadeInScene = FindObjectOfType<FadeInScene>();
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
        player.AilionMeet();
        Ailion.gameObject.SetActive(true);
        Enemy[] enemies = FindObjectsOfType<Enemy>(true);
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
            enemy.SliderActive(false);
        }
        AstarPath.active.Scan();
        Ailion.gameObject.SetActive(true);
        Ailion.ChaseStart(player);
        Destroy(InventoryManager.Instance.gameObject);
        player.cursorUI.SetCursorImage();
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
        enemies = FindObjectsOfType<Enemy>(true);
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
        if (Input.GetKey(KeyCode.R) && Input.GetKey( KeyCode.LeftControl))
        {
            OnDoorActive();
            player.transform.localPosition = new Vector3(-137.4f,5.4f, 92.7f);
        }
    }
    public bool isGameOver = false;
    public void GameOver()
    {
        isGameOver = true;
        foreach (Enemy enemy in enemies)
        {
            enemy.StopMove();
        }
        StartCoroutine(GameRestart());
    }
    Vector3 savePos;
    public void SavePos()
    {
        savePos = player.transform.position;
    }
    IEnumerator GameRestart()
    {
        yield return new WaitForSeconds(1.5f);
        fadeInScene.FadeIn();
        yield return new WaitUntil(() => fadeInScene.IsFadeStart());
        foreach (Enemy enemy in enemies)
        {
            enemy.InitNode();
            enemy.StopMove();
            enemy.transform.position = enemy.startPos;
            enemy.transform.rotation = enemy.startRot;
        }
        player.transform.position = savePos;
        player.transform.rotation = Quaternion.identity;
        player.animator.SetBool("Die", false);
        yield return new WaitUntil(() => fadeInScene.IsFadeEnd());
        foreach (Enemy enemy in enemies)
        {
            enemy.InitNode();
        }
        isGameOver = false;
        player.restart(); 
    }
}
