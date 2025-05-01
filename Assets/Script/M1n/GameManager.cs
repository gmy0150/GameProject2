using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject computer,Btn;
     
    public List<GameObject> AllPC = new List<GameObject>();
    bool OnComputer,DoorBtn = false;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            }
            return _instance;
        }
    }
    public int PcCount(){
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
        DontDestroyOnLoad(gameObject);
    }
    public bool AbleComputer(){
        return OnComputer;
    }
    public void OnComputerActive(){
        Debug.Log("컴퓨터 켜짐");
        OnComputer = true;
        computer.layer = 12;
        if(!computer.GetComponent<ComputerBtn>())
        computer.AddComponent<ComputerBtn>();
    }
    public bool AbleButton(){
        return DoorBtn;
    }
    public void OnDoorActive(){
        DoorBtn = true;
        Debug.Log("작3동함?");
        Btn.layer = 12;
        if(!Btn.GetComponent<DoorBtn>())
        Btn.AddComponent<DoorBtn>();
        
    }
    bool AIlionON = false;
    [SerializeField] GameObject wallexit;
    public void OnAilion(){
wallexit.SetActive(false);
AIlionON = true;
    }
    public bool AbleExit(){
        return AIlionON;
    }
}
