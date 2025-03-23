using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    public Light[] lights;

    public Color color1 = Color.red;
    public Color color2;
    public float duration = 1;
    float timer;
    private void Update()
    {
        
    }
    public void WorkLight()
    {
        Debug.Log("작동하나");
        timer = Mathf.PingPong(Time.time / duration, 1);
        Color newColor = Color.Lerp(color1, color2, timer);
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = true;
                light.color = newColor;
            }
        }
    }

}
