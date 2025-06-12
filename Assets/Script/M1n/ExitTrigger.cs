using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject parnel;
    public GameObject cartoonPrefab; 

    void OnTriggerEnter(Collider other)
    {
        if(!GameManager.Instance.AbleExit())
        return;

        Clear();
    }

    void Clear()
    {
        Debug.Log("Exit On");

        if (cartoonPrefab != null)
        {
            Instantiate(cartoonPrefab);
        }
        Time.timeScale = 0;
    }
}