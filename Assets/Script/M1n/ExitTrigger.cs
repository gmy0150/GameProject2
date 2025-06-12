using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject parnel;
    public GameObject cartoonPrefab;

    [Header("🎵 엔딩 BGM")]
    public AudioClip endingBGM;

    void OnTriggerEnter(Collider other)
    {
        if(!GameManager.Instance.AbleExit())
        return;

        Clear();
    }

    void Clear()
    {
        Debug.Log("Exit On");

        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.FadeToBGM(endingBGM);
        }

        if (cartoonPrefab != null)
        {
            Instantiate(cartoonPrefab);
        }
        Time.timeScale = 0;
    }
}