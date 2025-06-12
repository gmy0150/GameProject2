using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInScene : MonoBehaviour
{
    [Header("🔲 페이드용 패널 이미지 (검정 이미지)")]
    public Image panelImage;

    [Header("⏱️ 페이드 지속 시간")]
    public float fadeDuration = 1f;
    bool isFadeInEnd = false;
    bool isFadeOutEnd = false;
    private void Start()
    {
        if (panelImage == null)
        {
            Debug.LogError("FadeScript 오류: panelImage가 할당되지 않았습니다!");
            return;
        }

        // 시작 시 페이드 아웃 효과
        panelImage.color = new Color(0, 0, 0, 1);
        panelImage.DOFade(0f, fadeDuration).SetUpdate(true);
    }

    public void FadeIn()
    {
        isFadeInEnd = false;
        isFadeOutEnd = false;
        panelImage.gameObject.SetActive(true);
        panelImage.color = new Color(0, 0, 0, 0); // 투명으로 시작

        panelImage.DOFade(1f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                isFadeInEnd = true;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    panelImage.DOFade(0f, fadeDuration)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            panelImage.gameObject.SetActive(false);
                            isFadeOutEnd = true;
                        });
                });
            });
    }
    public bool IsFadeEnd()
    {
        return isFadeOutEnd;
    }
    public bool IsFadeStart()
    {
        return isFadeInEnd;
    }
}
