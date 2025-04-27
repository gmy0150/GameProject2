using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;     // 일시정지 메뉴 UI
    public GameObject gameUIRoot;    // 게임 중 UI 전체 (인벤토리, 체력바 등)

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        if (gameUIRoot != null)
            gameUIRoot.SetActive(false); // 게임 UI 전체 숨기기

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        if (gameUIRoot != null)
            gameUIRoot.SetActive(true); // 게임 UI 다시 보이기

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
    }
}
