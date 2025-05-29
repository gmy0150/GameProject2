using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameUIRoot;
    public GameObject optionMenu;
    public GameObject soundMenu;
    public GameObject screenMenu;
    public FadeScript fadeScript;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1. 사운드 메뉴 열려 있으면 → 닫고 옵션 메뉴만 보이게
            if (soundMenu != null && soundMenu.activeSelf)
            {
                soundMenu.SetActive(false);
                optionMenu.SetActive(true);
                Debug.Log("[ESC] 사운드 메뉴 닫음 → 옵션 메뉴 유지");
                return;
            }

            // 2. 화면 메뉴 열려 있으면 → 닫고 옵션 메뉴만 보이게
            if (screenMenu != null && screenMenu.activeSelf)
            {
                screenMenu.SetActive(false);
                optionMenu.SetActive(true);
                Debug.Log("[ESC] 화면 메뉴 닫음 → 옵션 메뉴 유지");
                return;
            }

            // 3. 옵션 메뉴 열려 있으면 → 닫고 pauseMenu 다시 열기
            if (optionMenu != null && optionMenu.activeSelf)
            {
                optionMenu.SetActive(false);
                pauseMenu.SetActive(true);
                Debug.Log("[ESC] 옵션 메뉴 닫고 → pauseMenu 복귀");
                return;
            }

            // 4. 패널 메뉴 열려 있으면 → 닫고 게임 복귀
            if (pauseMenu != null && pauseMenu.activeSelf)
            {
                ResumeGame();
                Debug.Log("[ESC] pauseMenu 닫고 → 게임 재개");
                return;
            }

            // 5. 아무 메뉴도 안 열려 있으면 → pauseMenu 열기
            PauseGame();
            Debug.Log("[ESC] 메뉴 없음 → pauseMenu 열기");
        }
    }




    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        if (gameUIRoot != null)
            gameUIRoot.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        if (gameUIRoot != null)
            gameUIRoot.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 대화창이 열려있는지 검사
        GameObject tutorialDialogue = GameObject.Find("Tutorial_Manager/Canvas");
        GameObject photoDialogue = GameObject.Find("Photo_Manager/Canvas");
        GameObject messageDialogue = GameObject.Find("Message_Manager/Canvas");

        bool isTutorialActive = tutorialDialogue != null && tutorialDialogue.activeInHierarchy;
        bool isPhotoActive = photoDialogue != null && photoDialogue.activeInHierarchy;
        bool isMessageActive = messageDialogue != null && messageDialogue.activeInHierarchy;

        Debug.Log($"[체크] Tutorial_Manager active = {isTutorialActive}");
        Debug.Log($"[체크] Message_Manager active = {isMessageActive}");
        Debug.Log($"[체크] Photo_Manager active = {isPhotoActive}");

        bool isDialogueActive = isTutorialActive || isMessageActive || isPhotoActive;

        isPaused = false;

        if (!isDialogueActive)
        {
            Time.timeScale = 1f;
            Debug.Log("[재개됨] 대화창 없음 → Time.timeScale = 1f");
        }
        else
        {
            Debug.Log("[중단 유지] 대화창이 열려있어 → Time.timeScale = 0f 유지됨");
        }
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (fadeScript != null)
        {
            fadeScript.FadeToScene("Test_MainMenu"); // 🔹 씬 이름 정확히!
        }
        else
        {
            Debug.LogWarning("FadeScript가 연결되지 않았습니다. 즉시 씬 로드합니다.");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
