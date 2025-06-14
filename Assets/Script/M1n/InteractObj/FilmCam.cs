using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EPOOutline;
using Unity.VisualScripting;

public class FilmCam : StorageItem
{
    Image Lights;
    private Player Player;
    List<GameObject> TakenPc = new List<GameObject>();

    public override void Init() { }
    public override void UpdateTime(float time)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interact))
        {
            Vector3 hitPos = hit.point;
            hitPos.y = Player.transform.position.y;
            float distance = Vector3.Distance(Player.transform.position, hitPos);
            if (distance > interactDis)
            {//interact보다 distance가 크면 return
                return;
            }
            GameObject hitobj = hit.collider.gameObject;
                if (TakenPc.Contains(hitobj)) return;
            Player.cursorUI.SetCursorImage(cursorImage);
        }
        else
        {
            Player.cursorUI.SetCursorImage(cusorInterectImage);
        }
    }
    public override void UseItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interact))
        {
            Vector3 hitPos = hit.point;
            hitPos.y = Player.transform.position.y;
            float distance = Vector3.Distance(Player.transform.position, hitPos);
            if (distance > interactDis)
            {//interact보다 distance가 크면 return
                return;
            }


            GameObject hitobj = hit.collider.gameObject;
            if (TakenPc.Contains(hitobj)) return;

            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (lookDir.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = targetRotation;
            }
            Player.SetAudio("Photo");
            GameManager.Instance.ActPlay(false);
            Lights.color = Color.white;
            Lights.transform.parent.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (picchildImage != null)
            {
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == hitobj.name)
                    {
                        picchildImage.sprite = sprite;
                        break;
                    }
                }
            }

            Lights.DOFade(0, 1).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                StartCoroutine(TakePicture(hitobj));
            });

            TakenPc.Add(hitobj);

            if (!hitobj.GetComponent<Outlinable>())
                hitobj.AddComponent<Outlinable>();

            Outlinable outlinable = hitobj.GetComponent<Outlinable>();
            outlinable.AddRenderer(hitobj.GetComponent<MeshRenderer>());
            outlinable.OutlineParameters.Enabled = true;
        }
    }
    public Sprite[] sprites;
    public GameObject AilionPc;

    IEnumerator TakePicture(GameObject go)
    {
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;
        GameManager.Instance.ActPlay(true);

        Lights.transform.parent.gameObject.SetActive(false);

        if (GameManager.Instance.PcCount() == TakenPc.Count)
        {
            GameManager.Instance.OnComputerActive();
        }
        Debug.Log($"go.name: {go.name}");  // ✅ 로그 ②
        Debug.Log($"AilionPc.name: {AilionPc.name}");  // ✅ 로그 ②
        if (go.name == AilionPc.name)
        {
            Debug.Log("작업해라");
            GameManager.Instance.ONAilionPic();
        }

        StartCoroutine(ShowDialogueDelayed(go.name));
    }

    IEnumerator ShowDialogueDelayed(string objName)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Debug.Log("[FilmCam] 호출된 오브젝트 이름: " + objName);  // ✅ 로그 ①
        PhotoTriggerManager.Instance.ShowDialogueFromObjectName(objName);
    }
    Image picchildImage;
    public override void inititem()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        interact = Player.Picture;
        Lights = Player.Lights;
        picchildImage = Player.PicChildImage;
        HandAnything = Player.Camera;
        SetHandActive(true);
        
    }
        public override void ActiveFalse()
    {
        SetHandActive(false);
        Player.cursorUI.SetCursorImage();
    }
}