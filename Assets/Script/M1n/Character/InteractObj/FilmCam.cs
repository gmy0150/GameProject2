using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FilmCam : StorageItem
{
    LayerMask picture;
    Image Lights;
    public Player Player;

    public void Start()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        picture = Player.Picture;
        Lights =Player.Lights;
        Debug.Log(picture);
    }

    public override void UseItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, picture))
        {
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
                StartCoroutine(TakePicture());
            });

            Debug.Log("사진 찍기!");
        }
    }
    
    IEnumerator TakePicture()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        Lights.transform.parent.gameObject.SetActive(false);
    }

    public override void inititem()
    {
    }

}
