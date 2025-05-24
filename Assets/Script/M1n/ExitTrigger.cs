using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject parnel;
    void OnTriggerEnter(Collider other)
    {
        if(!GameManager.Instance.AbleExit())
        return;
        Clear();
    }
    void Clear(){
        Debug.Log("?");
        parnel?.SetActive(true);
        Time.timeScale = 0;
    }
}
