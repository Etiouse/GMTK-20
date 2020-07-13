using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class UIHandler : MonoBehaviour
{
    public delegate void StartsdfEvent();
    public static event StartsdfEvent OnStartsdfEvent;

    [Header("Inner Scripts")]
    [SerializeField] private TimeSimulator timeSimulator = null;
    [SerializeField] private OptionsBar optionsBar = null;

    [Header("Objects")]
    [SerializeField] private GameObject wordObject = null;
    [SerializeField] private Button minimizeButton = null;
    [SerializeField] private GameObject gameOverWindow = null;
    [SerializeField] private TMP_Text gameOverMessage = null;
    [SerializeField] private GameObject victoryWindow = null;
    [SerializeField] private GameObject startOptionsWindow = null;
    [SerializeField] private Button saveButton = null;
    [SerializeField] private TMP_InputField jamThemes = null;

    [Header("Popups")]
    [SerializeField] private GameObject exitConfirmationWindow = null;
    [SerializeField] private GameObject saveConfirmationWindow = null;
    [SerializeField] private GameObject progressWindow = null;
    [SerializeField] private GameObject progressConfirmationWindow = null;

    [Header("Parameters")]
    [SerializeField] private Color minimizedColor = Color.white;
    [SerializeField] private Color maximizedColor = Color.white;
    [SerializeField] private float minimizeDelay = 1f;

    [Header("Others")]
    [SerializeField] private GameObject wordGame = null;
    [SerializeField] private GameObject wordGamePanel = null;
    [SerializeField] private Camera camera = null;

    private enum MinimizeState { IDLE, OPENING, CLOSING };
    private MinimizeState minState;

    private enum SaveState { FIRST_ATTEMPT, SECOND_ATTEMPT, THIRD_ATTEMPT, END };
    private SaveState saveState;

    private float stateChangedStart;
    private RectTransform wordRect;
    private Vector2 wordDim;
    private bool minimized;

    private string savedThemes;
    private bool eraseThemes;
    private float delay = 0.1f;
    private float lastErase;

    private float timeSavedTaken;
    private bool saveTaken;

    private float timeErased;
    private bool fileErased;

    private bool firstGameFinished;
    private bool gameFinished;

    private bool firstTimeFileOpenAfterMaze;

    public void Save()
    {
        saveConfirmationWindow.transform.parent.gameObject.SetActive(true);
        saveState = SaveState.FIRST_ATTEMPT;

        if (gameFinished)
        {
            saveConfirmationWindow.GetComponent<SaveWindow>().ShiftWindow();
            saveConfirmationWindow.GetComponent<SaveWindow>().SwapButtons();
        }
    }

    public void ValidateSave()
    {
        if (gameFinished)
        {
            switch (saveState)
            {
                case SaveState.FIRST_ATTEMPT:

                    saveConfirmationWindow.GetComponent<SaveWindow>().ShiftYes();

                    Clippy.Instance.ChangeState(Clippy.State.NORMAL);
                    Clippy.Instance.ChangePos(new Vector3(0, 0, 0));
                    Clippy.Instance.ChangeText("lmao, just kidding. Go for it :)", false);
                    Clippy.Instance.Show(10);

                    saveState = SaveState.SECOND_ATTEMPT;
                    break;
                case SaveState.SECOND_ATTEMPT:
                    Victory();

                    ExitSave();
                    Clippy.Instance.Hide();
                    break;
            }
        }
        else
        {
            switch (saveState)
            {
                case SaveState.FIRST_ATTEMPT:
                    // First attempt just shift the window upward
                    saveConfirmationWindow.GetComponent<SaveWindow>().ShiftWindow();
                    saveState = SaveState.SECOND_ATTEMPT;

                    AudioManager.instance.PlayGlitch();
                    break;
                case SaveState.SECOND_ATTEMPT:
                    // Second attempt swaps buttons and shift them downward
                    saveConfirmationWindow.GetComponent<SaveWindow>().SwapButtons();
                    saveState = SaveState.THIRD_ATTEMPT;

                    AudioManager.instance.PlayGlitch();

                    Clippy.Instance.ChangeState(Clippy.State.NORMAL);
                    Clippy.Instance.ChangePos(new Vector3(200, -150, 0));
                    Clippy.Instance.ChangeText("You know, you have to click on the button, not just randomly on the screen...", false);
                    Clippy.Instance.Show(10);
                    break;
                case SaveState.THIRD_ATTEMPT:
                    // Third attempt shift yes button upward
                    saveConfirmationWindow.GetComponent<SaveWindow>().ShiftYes();
                    saveState = SaveState.END;

                    AudioManager.instance.PlayGlitch();
                    break;
                case SaveState.END:
                    // Last attempt open progress window
                    ExitSave();
                    progressWindow.transform.parent.gameObject.SetActive(true);
                    progressWindow.GetComponent<ProgressWindow>().StartProgress();
                    Clippy.Instance.Hide();
                    break;
            }
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
        else if (saveState == SaveState.SECOND_ATTEMPT)
        {
            saveConfirmationWindow.GetComponent<SaveWindow>().ShiftBack();
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

    // Start boss events
    public void EndSavingProcess()
    {
        progressWindow.transform.parent.gameObject.SetActive(false);
        progressConfirmationWindow.transform.parent.gameObject.SetActive(false);

        optionsBar.ShowHomeBar();
        saveButton.gameObject.SetActive(false);

        firstGameFinished = true;

        Clippy.Instance.ChangeState(Clippy.State.EVIL);
        Clippy.Instance.ChangePos(new Vector3(-200, -200, 0));
        Clippy.Instance.ChangeText("Impressive, you reached the end of the maze. Sadly for you, you won't be able to save your work. But you can try.", false);
        Clippy.Instance.Show(10);

        timeSimulator.ChangeState(TimeSimulator.State.CORRUPTED);
    }

    public void StartStolenEvent()
    {
        if (firstGameFinished &&
            !firstTimeFileOpenAfterMaze)
        {
            firstTimeFileOpenAfterMaze = true;

            Clippy.Instance.ShowSave(true);
            Clippy.Instance.Show(15);
            Clippy.Instance.ChangePos(new Vector3(0, -220, 0));
            Clippy.Instance.ChangeState(Clippy.State.EVIL);
            Clippy.Instance.ChangeText("Is this what you're looking for ? Too bad, I'm taking it away from you.", false);

            saveTaken = true;
            timeSavedTaken = Time.time;
        }
    }

    private void StartErasing()
    {
        savedThemes = jamThemes.text;
        jamThemes.interactable = false;
        eraseThemes = true;

        Clippy.Instance.ShowSave(false);
        Clippy.Instance.Show(15);
        Clippy.Instance.ChangeState(Clippy.State.EVIL);
        Clippy.Instance.ChangePos(new Vector3(200, -220, 0));
        Clippy.Instance.ChangeText("Did you really think you did it ? Let's see if you appreciate all your work getting erased under your eyes", false);
    }

    private void StartGame()
    {
        Clippy.Instance.Show(0);
        Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
        Clippy.Instance.ChangePos(new Vector3(0, -220, 0));
        Clippy.Instance.ChangeText("I don't have much time until I lose it again. Quick, press on this !", false);
        Clippy.Instance.ShowDefense(true);
    }

    public void RunGame()
    {
        Clippy.Instance.ShowDefense(false);
        Clippy.Instance.Hide();

        wordGame.SetActive(true);
        wordGamePanel.SetActive(true);

        AudioManager.instance.PlayWordGameAmbiance();

        camera.backgroundColor = Color.black;
        gameObject.SetActive(false);
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

    public void ExitApp()
    {
        Application.Quit();
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
        ProgressWindow.OnProgressFinishedAction += OpenProgressValidationBait;
        WordGameHandler.OnWinWordGameEvent += WinWordGame;
    }

    private void OnDisable()
    {
        ProgressWindow.OnProgressFinishedAction -= OpenProgressValidationBait;
        WordGameHandler.OnWinWordGameEvent -= WinWordGame;
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

        AudioManager.instance.PlayStartSound();
    }

    private void Update()
    {
        // Save taken animation
        if (saveTaken)
        {
            if (Time.time - timeSavedTaken > 15)
            {
                saveTaken = false;
                StartErasing();
            }
        }

        // Erase themes animation
        if (eraseThemes)
        {
            if (Time.time - lastErase > delay)
            {
                lastErase = Time.time;

                if (jamThemes.text.Length > 0)
                {
                    jamThemes.text = jamThemes.text.Substring(0, jamThemes.text.Length - 2);
                }
                else
                {
                    eraseThemes = false;
                    jamThemes.interactable = true;

                    fileErased = true;
                    timeErased = Time.time;
                }
            }
        }

        // Start game
        if (fileErased && (Time.time - timeErased > 10))
        {
            fileErased = false;
            StartGame();
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

        AudioManager.instance.StopAmbiance();
        AudioManager.instance.PlayGameOverSound();
    }

    private void Victory()
    {
        victoryWindow.SetActive(true);

        AudioManager.instance.PlayVictorySound();

        camera.orthographicSize = 210;

        OnStartsdfEvent();

        gameObject.SetActive(false);
    }

    private void ResetGame()
    {
        AudioManager.instance.PlayIntroAmbiance();
        SceneManager.LoadScene("MainScene");
    }

    private void WinWordGame()
    {
        wordGame.SetActive(false);
        wordGamePanel.SetActive(false);

        Clippy.Instance.ChangeState(Clippy.State.EVIL);
        Clippy.Instance.ChangePos(new Vector3(200, -200, 0));
        Clippy.Instance.ChangeText("What is this?! An antivirus? AAAAAH IT HURTS!", false);
        Clippy.Instance.Show(10);

        StartCoroutine(StartWaitForEndBadClippy());

        GlitchEffect.instance.SetGlitch(1, 1, 0.3f);
        AudioManager.instance.PlayGlitch();
    }

    private IEnumerator StartWaitForEndBadClippy()
    {
        timeSimulator.ChangeState(TimeSimulator.State.TROUBLED);

        yield return new WaitForSeconds(10);

        AudioManager.instance.PlayIntroAmbiance();
        timeSimulator.ChangeState(TimeSimulator.State.NORMAL);

        Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
        Clippy.Instance.ChangePos(new Vector3(-200, -200, 0));
        Clippy.Instance.ChangeText("Wow... Is it over? ", false);
        Clippy.Instance.Show(10);

        yield return new WaitForSeconds(10);

        Clippy.Instance.ChangeState(Clippy.State.NORMAL);
        Clippy.Instance.ChangePos(new Vector3(0, 0, 0));
        Clippy.Instance.ChangeText("Let me check if I can restore your work...", false);
        Clippy.Instance.Show(10);

        yield return new WaitForSeconds(10);

        Clippy.Instance.ChangeState(Clippy.State.NORMAL);
        Clippy.Instance.ChangePos(new Vector3(-200, 200, 0));
        Clippy.Instance.ChangeText("I think I found it.", false);
        Clippy.Instance.Show(10);

        yield return new WaitForSeconds(5);

        char[] charArray = savedThemes.ToCharArray();
        Array.Reverse(charArray);
        jamThemes.text = new string(charArray);

        yield return new WaitForSeconds(5);

        Clippy.Instance.ChangeState(Clippy.State.TROUBLED);
        Clippy.Instance.ChangePos(new Vector3(200, -200, 0));
        Clippy.Instance.ChangeText("Oh... Something is wrong. Let me correct that", false);
        Clippy.Instance.Show(10);

        yield return new WaitForSeconds(10);

        jamThemes.text = savedThemes;

        Clippy.Instance.ChangeState(Clippy.State.NORMAL);
        Clippy.Instance.ChangePos(new Vector3(0, 0, 0));
        Clippy.Instance.ChangeText("Yeah! Much better. I think your file can be saved now.", false);
        Clippy.Instance.Show(10);

        saveButton.gameObject.SetActive(true);

        gameFinished = true;
    }
}
