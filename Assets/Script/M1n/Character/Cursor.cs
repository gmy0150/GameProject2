using UnityEngine;
using UnityEngine.UI;
public class CursorUI
{
    public Image customCursorUI;
    Sprite baseImage;
    Sprite BaseAbleIamage;

    public void Start()
    {
        customCursorUI = GameObject.Find("CustomCursor").GetComponent<Image>();
        baseImage = Resources.Load<Sprite>("UI/Mouse/DefaultCursor");
        BaseAbleIamage = Resources.Load<Sprite>("UI/Mouse/DefaultAbleCursor");
        
    }
    public void update()
    {
        Vector3 mousePos = Input.mousePosition;
        customCursorUI.rectTransform.position = mousePos;
    }
    public void SetCursorImage(Sprite sprite = null, bool actImage = true)
    {

        if (sprite == null)
        {
            if (actImage)
            {
                customCursorUI.sprite = baseImage;
            }
            else
            {
                customCursorUI.sprite = BaseAbleIamage;
            }
        }
        else
        {
            customCursorUI.sprite = sprite;
        }
    }
}