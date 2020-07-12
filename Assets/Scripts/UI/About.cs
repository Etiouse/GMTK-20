using UnityEngine;

public class About : MonoBehaviour
{
    [SerializeField] private GameObject aboutWindow = null;

    public void Show()
    {
        aboutWindow.SetActive(true);
    }

    public void Hide()
    {
        aboutWindow.SetActive(false);
    }
}
