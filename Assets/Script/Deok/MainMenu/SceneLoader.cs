using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // 씬 이름에 맞게 설정하면됨. ( 추후에 수정 )
    }
}
