using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressWindow : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private Button cancelButton = null;
    [SerializeField] private Button finishButton = null;
    [SerializeField] private TMP_Text percentage = null;
    [SerializeField] private TMP_Text timeLeft = null;
    [SerializeField] private float savingDuration = 90f;

    public delegate void ProgressFinishedAction();
    public event ProgressFinishedAction OnProgressFinishedAction;

    private float timeStarted;
    private bool on;

    public void StartProgress()
    {
        timeStarted = Time.time;
        on = true;
    }

    public void ShowFinish()
    {
        finishButton.gameObject.SetActive(true);
    }

    public void ResetProgress()
    {
        on = false;
        slider.value = 0;
    }

    private void Start()
    {
        slider.value = 0;
        finishButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (on)
        {
            float timePassed = Time.time - timeStarted;
            float ratio = timePassed / savingDuration;
            int left = (int)(savingDuration - timePassed);

            if (timePassed <= savingDuration)
            {
                percentage.text = (int)(100 * ratio) + "%";
                timeLeft.text = "Estimated time left: " + left + " seconds";
                slider.value = ratio;
            }
            else
            {
                percentage.text = "100%";
                timeLeft.text = "Estimated time left: 0 seconds";
                slider.value = 1f;

                on = false;

                OnProgressFinishedAction();

                cancelButton.interactable = false;
            }
        }
    }
}
