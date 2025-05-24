using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SceneMoveBtn : UseageInteract
{
    public Image panelImage;
    public float delayDuration = 3f;  
    public float fadeDuration = 1.5f;     

    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        Debug.Log("눌림");

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

    public override bool IsOneTimeInteraction()
    {
        return true;
    }

    public override void UpdateTime(float time)
    {
        throw new System.NotImplementedException();
    }
}
