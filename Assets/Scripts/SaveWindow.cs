using UnityEngine;

public class SaveWindow : MonoBehaviour
{
    [SerializeField] private GameObject optionsParent = null;

    public void ShiftWindow()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + 35);
    }

    public void SwapButtons()
    {
        for (int i = 0; i < optionsParent.transform.childCount; i++)
        {
            optionsParent.transform.GetChild(i).SetAsFirstSibling();
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - 35);
    }

    public void SwapBack()
    {
        for (int i = 0; i < optionsParent.transform.childCount; i++)
        {
            optionsParent.transform.GetChild(i).SetAsFirstSibling();
        }
    }

    public void ShiftYes()
    {
        RectTransform rectTransform = optionsParent.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - 100);
    }
}
