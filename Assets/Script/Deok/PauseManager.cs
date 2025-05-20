using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameUIRoot;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
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
}
