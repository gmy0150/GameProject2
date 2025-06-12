using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SceneMoveBtn : MonoBehaviour
{
    public Image panelImage;
    public float delayDuration = 3f;
    public float fadeDuration = 1.5f;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Second_Scene");
            if (prefab != null)
            {
                Instantiate(prefab);
            }

            if (panelImage != null)
            {
                panelImage.gameObject.SetActive(true);
                panelImage.color = new Color(0f, 0f, 0f, 1f);

                DOVirtual.DelayedCall(delayDuration, () =>
                {
                    panelImage.DOFade(0f, fadeDuration).OnComplete(() =>
                    {
                        panelImage.gameObject.SetActive(false);
                    });
                });
            }
        }
    }
}
