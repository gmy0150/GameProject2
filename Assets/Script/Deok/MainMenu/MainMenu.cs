using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public FadeScript fadeScript;  // 인스펙터에 연결

    public void Play()
    {
        fadeScript.FadeToScene("M_tuto"); // 씬 이름 전달
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Player Has Quit the Game.");
#endif
    }

    public void OpenOptions()
    {
        Debug.Log("Options Opened");
    }
}
