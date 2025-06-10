using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("업적 오브젝트")]
    public GameObject[] achievementSteps;

    [Header("업적 체크 이모트")]
    public GameObject[] emotes;

    [Header("1번 업적 관련")]
    public TMP_Text countText;
    public GameObject[] secrets;

    [Header("4번 업적 관련")]
    public GameObject finalPhotoTarget;

    private int secretsFound = 0;
    private int currentStep = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateAchievements();
        UpdateSecretProgress();
        HideAllEmotes();
    }
    
    public bool FoundSecret()
    {
        if (secretsFound >= 4) return false;
        secretsFound++;
        UpdateSecretProgress();
        if (secretsFound == 4) return true;
        return false;
    }

    public void TriggerFirstQuestCompletion()
    {
        StartCoroutine(CompleteFirstQuestSequence());
    }

    private IEnumerator CompleteFirstQuestSequence()
    {
        if (currentStep == 0)
        {
            Toggle checkbox = achievementSteps[currentStep].transform.Find("CheckBox")?.GetComponent<Toggle>();
            if (checkbox != null) checkbox.isOn = true;
            if (currentStep < emotes.Length && emotes[currentStep] != null) emotes[currentStep].SetActive(true);
        }

        bool dialogueFinished = false;
        PhotoTriggerManager.Instance.ShowDialogueFromObjectName("Picture complete", () => { dialogueFinished = true; });
        yield return new WaitUntil(() => dialogueFinished);

        if (currentStep + 1 < achievementSteps.Length)
        {
            TMP_Text nextChat = achievementSteps[currentStep + 1].transform.Find("CheckBox/CheckChat")?.GetComponent<TMP_Text>();
            if (nextChat != null) nextChat.text = "집의 메인 컴퓨터를 이용하여 비밀을 찾아내시오.";
        }
        currentStep++;
        UpdateAchievements();
    }

    // [수정] 2번 체크박스: 이제 코루틴을 시작합니다.
    public void CompleteComputerMission()
    {
        if (currentStep == 1)
            StartCoroutine(CompleteSecondQuestSequence());
    }

    // [추가] 2번 퀘스트 완료 시퀀스 코루틴
    private IEnumerator CompleteSecondQuestSequence()
    {
        if (currentStep == 1)
        {
            Toggle checkbox = achievementSteps[currentStep].transform.Find("CheckBox")?.GetComponent<Toggle>();
            if (checkbox != null) checkbox.isOn = true;
            if (currentStep < emotes.Length && emotes[currentStep] != null) emotes[currentStep].SetActive(true);
        }

        bool dialogueFinished = false;
        PhotoTriggerManager.Instance.ShowDialogueFromObjectName("Computer complete", () => { dialogueFinished = true; });
        yield return new WaitUntil(() => dialogueFinished);

        if (currentStep + 1 < achievementSteps.Length)
        {
            TMP_Text nextChat = achievementSteps[currentStep + 1].transform.Find("CheckBox/CheckChat")?.GetComponent<TMP_Text>();
            if (nextChat != null) nextChat.text = "집의 비밀버튼을 찾아 작동하고 비밀을 밝혀내세요!";
        }
        currentStep++;
        UpdateAchievements();
    }

    // [수정] 3번 체크박스: 이제 코루틴을 시작합니다.
    public void CompleteSecretButtonMission()
    {
        if (currentStep == 2)
            StartCoroutine(CompleteThirdQuestSequence());
    }

    // [추가] 3번 퀘스트 완료 시퀀스 코루틴
    private IEnumerator CompleteThirdQuestSequence()
    {
        if (currentStep == 2)
        {
            Toggle checkbox = achievementSteps[currentStep].transform.Find("CheckBox")?.GetComponent<Toggle>();
            if (checkbox != null) checkbox.isOn = true;
            if (currentStep < emotes.Length && emotes[currentStep] != null) emotes[currentStep].SetActive(true);
        }

        bool dialogueFinished = false;
        PhotoTriggerManager.Instance.ShowDialogueFromObjectName("Button complete", () => { dialogueFinished = true; });
        yield return new WaitUntil(() => dialogueFinished);
        
        if (currentStep + 1 < achievementSteps.Length)
        {
            TMP_Text nextChat = achievementSteps[currentStep + 1].transform.Find("CheckBox/CheckChat")?.GetComponent<TMP_Text>();
            if (nextChat != null) nextChat.text = "비밀의 방을 찾아 사진을 찍으세요.";
        }
        currentStep++;
        UpdateAchievements();
    }

    // 4번 체크박스 (변경 없음)
    public void CompleteFinalPhotoMission()
    {
        if (currentStep == 3)
        {
            CompleteCurrentQuest(); // 4번 퀘스트는 대화가 없으므로 기존 방식 유지
        }
    }
    
    // (이하 나머지 코드는 변경 없음)
    public void CompleteCurrentQuest()
    {
        if (currentStep >= achievementSteps.Length) return;
        Toggle checkbox = achievementSteps[currentStep].transform.Find("CheckBox")?.GetComponent<Toggle>();
        if (checkbox != null) checkbox.isOn = true;
        if (currentStep < emotes.Length && emotes[currentStep] != null) emotes[currentStep].SetActive(true);
        // ...
        currentStep++;
        UpdateAchievements();
    }
    void UpdateSecretProgress() { countText.text = $"집 안에 숨기고 있는 비밀을 찾아내십시오. ({secretsFound}/4)"; }
    void UpdateAchievements() { for (int i = 0; i < achievementSteps.Length; i++) achievementSteps[i].SetActive(i <= currentStep); }
    public bool IsSecretObjectByName(string objName) { foreach (var secret in secrets) { if (secret != null && secret.name == objName) return true; } return false; }
    public bool IsFinalPhotoTarget(string objName) { return finalPhotoTarget != null && finalPhotoTarget.name == objName; }
    void HideAllEmotes() { foreach (var emote in emotes) { if (emote != null) emote.SetActive(false); } }
}