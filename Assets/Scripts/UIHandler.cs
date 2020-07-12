using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("Inner Scripts")]
    [SerializeField] private TimeSimulator timeSimulator = null;

    [Header("Objects")]
    [SerializeField] private GameObject wordObject = null;
    [SerializeField] private Button minimizeButton = null;
    [SerializeField] private GameObject gameOverWindow = null;
    [SerializeField] private TMP_Text gameOverMessage = null;
    [SerializeField] private GameObject startOptionsWindow = null;

    [Header("Popups")]
    [SerializeField] private GameObject exitConfirmationWindow = null;
    [SerializeField] private GameObject saveConfirmationWindow = null;
    [SerializeField] private GameObject progressWindow = null;
    [SerializeField] private GameObject progressConfirmationWindow = null;

    [Header("Parameters")]
    [SerializeField] private Color minimizedColor = Color.white;
    [SerializeField] private Color maximizedColor = Color.white;
    [SerializeField] private float minimizeDelay = 1f;
    
    private enum MinimizeState { IDLE, OPENING, CLOSING };
    private MinimizeState minState;

    private enum SaveState { FIRST_ATTEMPT, SECOND_ATTEMPT, THIRD_ATTEMPT, END };
    private SaveState saveState;

    private float stateChangedStart;
    private RectTransform wordRect;
    private Vector2 wordDim;
    private bool minimized;

    public void Save()
    {
        saveConfirmationWindow.transform.parent.gameObject.SetActive(true);
    }

    public void ValidateSave()
    {
        switch (saveState)
        {
            case SaveState.FIRST_ATTEMPT:
                // First attempt just shift the window upward
                saveConfirmationWindow.GetComponent<SaveWindow>().ShiftWindow();
                saveState = SaveState.SECOND_ATTEMPT;
                break;
            case SaveState.SECOND_ATTEMPT:
                // Second attempt swaps buttons and shift them downward
                saveConfirmationWindow.GetComponent<SaveWindow>().SwapButtons();
                saveState = SaveState.THIRD_ATTEMPT;
                break;
            case SaveState.THIRD_ATTEMPT:
                // Third attempt shift yes button upward
                saveConfirmationWindow.GetComponent<SaveWindow>().ShiftYes();
                saveState = SaveState.END;
                break;
            case SaveState.END:
                // Last attempt open progress window
                ExitSave();
                progressWindow.transform.parent.gameObject.SetActive(true);
                progressWindow.GetComponent<ProgressWindow>().StartProgress();
                break;
        }
    }

    public void ExitProgress()
    {
        progressWindow.transform.parent.gameObject.SetActive(false);
        progressWindow.GetComponent<ProgressWindow>().ResetProgress();
    }

    public void ExitSave()
    {
        if (saveState >= SaveState.THIRD_ATTEMPT)
        {
            saveConfirmationWindow.GetComponent<SaveWindow>().SwapBack();
        }

        saveConfirmationWindow.transform.parent.gameObject.SetActive(false);
        saveState = SaveState.FIRST_ATTEMPT;
    }

    public void Exit()
    {
        exitConfirmationWindow.transform.parent.gameObject.SetActive(true);
    }

    public void ExitExit()
    {
        exitConfirmationWindow.transform.parent.gameObject.SetActive(false);
    }

    public void ExitAndSave()
    {
        ExitExit();
        Save();
    }

    public void ExitWithoutSaving()
    {
        ExitExit();
        GameOver("You closed Wrod without saving your document");
    }

    public void EndSavingProcess()
    {
        progressWindow.transform.parent.gameObject.SetActive(false);
        progressConfirmationWindow.transform.parent.gameObject.SetActive(false);
    }

    public void Shutdown()
    {
        GameOver("You stopped your computer without saving your document");
    }

    public void InitCorruption()
    {
        timeSimulator.ChangeState(TimeSimulator.State.CORRUPTED);
    }

    public void Retry()
    {
        ResetGame();
    }

    public void ShowStartOptions()
    {
        if (startOptionsWindow.activeSelf)
        {
            startOptionsWindow.SetActive(false);
        }
        else
        {
            startOptionsWindow.SetActive(true);
        }
    }

    public void Minimize()
    {
        minimized = true;
        minimizeButton.GetComponent<Image>().color = minimized ? minimizedColor : maximizedColor;

        minState = MinimizeState.CLOSING;
        stateChangedStart = Time.time;
    }

    public void MinimizedClicked()
    {
        minimized = !minimized;
        minimizeButton.GetComponent<Image>().color = minimized ? minimizedColor : maximizedColor;

        if (minimized)
        {
            minState = MinimizeState.CLOSING;
        }
        else
        {
            minState = MinimizeState.OPENING;
        }
        stateChangedStart = Time.time;
    }

    private void OnEnable()
    {
        progressWindow.GetComponent<ProgressWindow>().OnProgressFinishedAction += OpenProgressValidationBait;
    }

    private void OnDisable()
    {
        progressWindow.GetComponent<ProgressWindow>().OnProgressFinishedAction -= OpenProgressValidationBait;
    }

    private void Start()
    {
        minState = MinimizeState.IDLE;
        saveState = SaveState.FIRST_ATTEMPT;
        stateChangedStart = 0f;

        wordRect = wordObject.GetComponent<RectTransform>();
        wordDim = new Vector2(wordRect.rect.width, wordRect.rect.height);
        minimized = false;

        startOptionsWindow.SetActive(false);
    }

    private void Update()
    {
        // Shortcuts
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Shortcuts with control key
            if (Input.GetKey(KeyCode.S))
            {
                Save();
            }
        }

        // Hide Start menu if clicked outside of the start button
        if (Input.GetMouseButton(0) && Input.mousePosition.x > 120 && Input.mousePosition.y > 30)
        {
            startOptionsWindow.SetActive(false);
        }

        // Animate minimization
        if (minState != MinimizeState.IDLE)
        {
            float dTime = Time.time - stateChangedStart;
            float ratio = 1f;

            if (dTime >= minimizeDelay)
            {
                // End ratio
                ratio = minState == MinimizeState.CLOSING ? 1 - ratio : ratio;

                // Stop animation
                wordObject.GetComponent<CanvasGroup>().interactable = minState == MinimizeState.OPENING;
                minState = MinimizeState.IDLE;
            }
            else
            {
                // Compute ratio
                ratio = dTime / minimizeDelay;
                ratio = minState == MinimizeState.CLOSING ? 1 - ratio : ratio;
            }

            wordRect.offsetMin = new Vector2(130 * (1 - ratio), wordRect.offsetMin.y);
            wordRect.offsetMax = new Vector2((350 - wordDim.x) * (1 - ratio), -wordDim.y * (1 - ratio));
            wordObject.GetComponent<CanvasGroup>().alpha = ratio;
        }
    }

    private void OpenProgressValidationBait()
    {
        progressConfirmationWindow.transform.parent.gameObject.SetActive(true);

        timeSimulator.ChangeState(TimeSimulator.State.TROUBLED);
    }

    private void GameOver(string message)
    {
        gameOverWindow.SetActive(true);
        gameOverMessage.text = message;
    }

    private void ResetGame()
    {
        gameOverWindow.SetActive(false);
        minimized = false;
        minState = MinimizeState.IDLE;
        timeSimulator.ChangeState(TimeSimulator.State.NORMAL);
        progressWindow.GetComponent<ProgressWindow>().ResetProgress();
        progressConfirmationWindow.GetComponent<BaitWindow>().ResetBait();
    }
}
