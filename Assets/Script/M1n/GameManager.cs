using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject computer;
     Outlinable outlinable;
     
    public List<GameObject> AllPC = new List<GameObject>();

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
    void Start()
    {
        outlinable = computer.GetComponent<Outlinable>();
    }
    bool OnComputer = true;
    public bool AbleComputer(){
        return OnComputer;
    }
    public void OnActive(){
        OnComputer = true;
        outlinable.OutlineParameters.Enabled = true;
        computer.layer = 12;
        computer.AddComponent<ComputerBtn>();
    }
}
