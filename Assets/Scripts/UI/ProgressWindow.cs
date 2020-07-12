using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressWindow : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private Button cancelButton = null;
    [SerializeField] private Button finishButton = null;
    [SerializeField] private Button exitButton = null;
    [SerializeField] private TMP_Text percentage = null;
    [SerializeField] private TMP_Text timeLeft = null;
    [SerializeField] private float savingDuration = 90f;

    public delegate void ProgressFinishedAction();
    public static event ProgressFinishedAction OnProgressFinishedAction;

    private float timeStarted;
    private bool on;

    private bool firstClippy;
    private bool secondClippy;
    private bool thirdClippy;
    private bool accelerateAccepted;
    private float accelerateAcceptedTime;

    public void Accelerate()
    {
        accelerateAccepted = true;
        accelerateAcceptedTime = Time.time;
    }

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
        finishButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        slider.value = 0;
        finishButton.gameObject.SetActive(false);

        firstClippy = true;
        secondClippy = true;
        thirdClippy = true;
    }

    private void Update()
    {
        if (on)
        {
            float timePassed = Time.time - timeStarted;
            if (accelerateAccepted)
            {
                timePassed = (accelerateAcceptedTime - timeStarted) + (Time.time - accelerateAcceptedTime) * 50;
            } 

            float ratio = timePassed / savingDuration;
            int left = (int)(savingDuration - timePassed);

            if (timePassed > 10 && firstClippy)
            {
                firstClippy = false;
                Clippy.Instance.Show(0);
                Clippy.Instance.ChangeText("It’s taking waaaay too long to save... I can try to speed up the process, do you want some help?", true);
                Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
                Clippy.Instance.ChangePos(new Vector3(200, -150, 0));
            }

            if (timePassed > 25 && secondClippy && !accelerateAccepted)
            {
                secondClippy = false;
                Clippy.Instance.Show(0);
                Clippy.Instance.ChangeText("Are you sure you don’t want my help? I think it will take ages otherwise.", true);
                Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
                Clippy.Instance.ChangePos(new Vector3(-250, 100, 0));
            }

            if (timePassed > 40 && thirdClippy && !accelerateAccepted)
            {
                thirdClippy = false;
                Clippy.Instance.Show(0);
                Clippy.Instance.ChangeText("Ok it’s taking too much time,  enough waiting. I’m gonna help you !", false);
                Clippy.Instance.ChangeState(Clippy.State.BAD);
                Clippy.Instance.ChangePos(new Vector3(200, -150, 0));

            }

            if (timePassed > 44 && !accelerateAccepted)
            {
                Accelerate();
            }

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
                exitButton.interactable = false;

                Clippy.Instance.Show(0);
                Clippy.Instance.ChangeText("What just happened?! I didn't mean to do that... I think you just have to follow the path and click on the confirm button to complete the save.", false);
                Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
                Clippy.Instance.ChangePos(new Vector3(200, -260, 0));
            }
        }
    }
}
