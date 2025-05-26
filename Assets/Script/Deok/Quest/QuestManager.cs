using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // 1번 체크박스
    public void FoundSecret()
    {
        if (secretsFound >= 4) return;

        secretsFound++;
        UpdateSecretProgress();

        if (secretsFound == 4)
        {
            CompleteCurrentQuest();
        }
    }

    // 2번 체크박스
    public void CompleteComputerMission()
    {
        if (currentStep == 1)
            CompleteCurrentQuest();
    }

    // 3번 체크박스
    public void CompleteSecretButtonMission()
    {
        if (currentStep == 2)
            CompleteCurrentQuest();
    }

    // 4번 체크박스
    public void CompleteFinalPhotoMission()
    {
        if (currentStep == 3)
            CompleteCurrentQuest();
    }
    public void CompleteCurrentQuest()
    {
        if (currentStep >= achievementSteps.Length) return;

        Toggle checkbox = achievementSteps[currentStep].transform.Find("CheckBox")?.GetComponent<Toggle>();
        if (checkbox != null)
            checkbox.isOn = true;

        if (currentStep < emotes.Length && emotes[currentStep] != null)
            emotes[currentStep].SetActive(true);

        // 다음 임무 업데이트트
        if (currentStep + 1 < achievementSteps.Length)
        {
            TMP_Text nextChat = achievementSteps[currentStep + 1].transform
                .Find("CheckBox/CheckChat")?.GetComponent<TMP_Text>();

            if (nextChat != null)
            {
                if (currentStep == 0)
                    nextChat.text = "집의 메인 컴퓨터를 이용하여 비밀을 찾아내시오.";
                else if (currentStep == 1)
                    nextChat.text = "집의 비밀버튼을 찾아 작동하고 비밀을 밝혀내세요!";
                else if (currentStep == 2)
                    nextChat.text = "비밀의 방을 찾아 사진을 찍으세요.";
            }
        }

        currentStep++;
        UpdateAchievements();
    }

    void UpdateSecretProgress()
    {
        countText.text = $"집 안에 숨기고 있는 비밀을 찾아내십시오. ({secretsFound}/4)";
    }

    void UpdateAchievements()
    {
        for (int i = 0; i < achievementSteps.Length; i++)
            achievementSteps[i].SetActive(i <= currentStep);
    }

    public bool IsSecretObjectByName(string objName)
    {
        foreach (var secret in secrets)
        {
            if (secret != null && secret.name == objName)
                return true;
        }
        return false;
    }

    public bool IsFinalPhotoTarget(string objName)
    {
        return finalPhotoTarget != null && finalPhotoTarget.name == objName;
    }

    void HideAllEmotes()
    {
        foreach (var emote in emotes)
        {
            if (emote != null)
                emote.SetActive(false);
        }
    }
}