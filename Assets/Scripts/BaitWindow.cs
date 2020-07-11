using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaitWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private Button button = null;

    public void Corrupt()
    {
        description.text = "GIVE ADMIN RIGHTS TO VIRUS";
        description.color = Color.red;
        button.interactable = false;
    }
}
