using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaitWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private Button button = null;

    public void Corrupt()
    {
        description.text = "GIVE ADMIN RIGHTS TO VIRUS ?!";
        description.color = Color.red;
        button.interactable = false;
        
        Clippy.Instance.Show(0);
        Clippy.Instance.ChangeText(" I took control of your PC and will erase aaaaaall the work you’ve done today Mwahahahahahahahahaha *cough*", false);
        Clippy.Instance.ChangeState(Clippy.State.EVIL);
    }

    public void ResetBait()
    {
        description.text = "VALIDATE SAVE PROCESS ?!";
        description.color = Color.black;
        button.interactable = true;
    }
}
