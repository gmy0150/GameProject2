using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EPOOutline;

public class FilmCam : StorageItem
{
    LayerMask picture;
    Image Lights;
    private Player Player;
    List<GameObject> TakenPc = new List<GameObject>();
    public override void Init()
    {

    }
    
    public override void UseItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, picture))
        {
            GameObject hitobj = hit.collider.gameObject;
            if(TakenPc.Contains(hitobj))
            return;
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (lookDir.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = targetRotation;
            }

            Lights.color = Color.white;
            Lights.transform.parent.gameObject.SetActive(true);
            Time.timeScale = 0;

            Lights.DOFade(0, 1).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                StartCoroutine(TakePicture(hitobj));
            });
            TakenPc.Add(hitobj);
            if(!hitobj.GetComponent<Outlinable>())
                hitobj.AddComponent<Outlinable>();
            Outlinable outlinable = hitobj.GetComponent<Outlinable>();
            outlinable.AddRenderer(hitobj.GetComponent<MeshRenderer>());
            outlinable.OutlineParameters.Enabled = true;
            Debug.Log("사진 찍기!");
        }
    }
    public AilionAI Ailion;
    public GameObject AilionPc;
    IEnumerator TakePicture(GameObject go)
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        Lights.transform.parent.gameObject.SetActive(false);
        if(GameManager.Instance.PcCount() == TakenPc.Count){
            GameManager.Instance.OnComputerActive();
        }
        if(go == AilionPc){
            Debug.Log("??");
            Ailion.gameObject.SetActive(true);
            Ailion.ChaseStart(Player);
            GameManager.Instance.OnAilion();
        }else{
            Debug.Log("");
        }
        Debug.Log(go);
    }

    public override void inititem()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        picture = Player.Picture;
        Lights =Player.Lights;
        Debug.Log(picture);
    }

}