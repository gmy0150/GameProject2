using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSetting : MonoBehaviour
{
    public Button X2;
    public Button X1;
    private void Start()
    {
        X2.onClick.AddListener(() => Time2X());
        X1.onClick.AddListener(() => Time1X());
    }
    public void Time1X()
    {
        Debug.Log("Å¬¸¯");
        Time.timeScale = 1f;
    }

    public void Time2X()
    {
        Time.timeScale = 2f;
    }
}
