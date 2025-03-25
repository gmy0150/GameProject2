using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeScript : MonoBehaviour
{
    public Image panelImage;
    public float fadeDuration = 1f;

    public void FadeToScene(string sceneName)
    {
        panelImage.gameObject.SetActive(true);
        panelImage.color = new Color(0, 0, 0, 0); // 시작은 투명

        panelImage.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
