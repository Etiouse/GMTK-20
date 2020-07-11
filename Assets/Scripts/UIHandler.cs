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

    [Header("Parameters")]
    [SerializeField] private Color minimizedColor = Color.white;
    [SerializeField] private Color maximizedColor = Color.white;
    [SerializeField] private float minimizeDelay = 1f;

    private enum minimizeState { IDLE, OPENING, CLOSING };
    private minimizeState minState;

    private float stateChangedStart;
    private RectTransform wordRect;
    private Vector2 wordDim;
    private bool minimized;

    public void Exit()
    {
        GameOver("You closed your document without saving it");
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

        minState = minimizeState.CLOSING;
        stateChangedStart = Time.time;
    }

    public void MinimizedClicked()
    {
        minimized = !minimized;
        minimizeButton.GetComponent<Image>().color = minimized ? minimizedColor : maximizedColor;

        if (minimized)
        {
            minState = minimizeState.CLOSING;
        }
        else
        {
            minState = minimizeState.OPENING;
        }
        stateChangedStart = Time.time;
    }

    private void Start()
    {
        minState = minimizeState.IDLE;
        stateChangedStart = 0f;

        wordRect = wordObject.GetComponent<RectTransform>();
        wordDim = new Vector2(wordRect.rect.width, wordRect.rect.height);
        minimized = false;

        startOptionsWindow.SetActive(false);
    }

    private void Update()
    {
        // Hide Start menu if clicked outside of the start button
        if (Input.GetMouseButton(0) && Input.mousePosition.x > 120 && Input.mousePosition.y > 30)
        {
            startOptionsWindow.SetActive(false);
        }

        // Animate minimization
        if (minState != minimizeState.IDLE)
        {
            float dTime = Time.time - stateChangedStart;
            float ratio = 1f;

            if (dTime >= minimizeDelay)
            {
                // End ratio
                ratio = minState == minimizeState.CLOSING ? 1 - ratio : ratio;

                // Stop animation
                wordObject.GetComponent<CanvasGroup>().interactable = minState == minimizeState.OPENING;
                minState = minimizeState.IDLE;
            }
            else
            {
                // Compute ratio
                ratio = dTime / minimizeDelay;
                ratio = minState == minimizeState.CLOSING ? 1 - ratio : ratio;
            }

            wordRect.offsetMin = new Vector2(130 * (1 - ratio), wordRect.offsetMin.y);
            wordRect.offsetMax = new Vector2((350 - wordDim.x) * (1 - ratio), -wordDim.y * (1 - ratio));
            wordObject.GetComponent<CanvasGroup>().alpha = ratio;
        }
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
        minState = minimizeState.IDLE;
    }
}
