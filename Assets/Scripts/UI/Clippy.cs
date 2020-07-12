using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clippy : MonoBehaviour
{
    [SerializeField] private GameObject visual = null;
    [SerializeField] private Transform imageTransform = null;
    [SerializeField] private Image clippy = null;
    [SerializeField] private Sprite normalClippy = null;
    [SerializeField] private Sprite troubledClippy = null;
    [SerializeField] private Sprite badClippy = null;
    [SerializeField] private Sprite evilClippy = null;
    [SerializeField] private TMP_Text message = null;
    [SerializeField] private Button acceptButton = null;
    [SerializeField] private Button rejectButton = null;
    [SerializeField] private GameObject saveImage = null;
    [SerializeField] private GameObject defenseButton = null;
    [SerializeField] private Transform refTransform = null;

    public static Clippy Instance;

    public enum State { NORMAL, TROUBLED, BAD, EVIL };
    private State state;

    private bool shown;
    private float duration;
    private float timeShown;

    public void ChangeState(State newState)
    {
        state = newState;

        clippy.overrideSprite = GetStateSprite();
    }

    public void ShowDefense(bool show)
    {
        defenseButton.SetActive(show);
    }

    public void ShowSave(bool show)
    {
        saveImage.SetActive(show);
    }

    public void ChangeText(string content, bool showButtons)
    {
        message.text = content;

        acceptButton.gameObject.SetActive(showButtons);
        rejectButton.gameObject.SetActive(showButtons);

        message.color = state == State.EVIL ? Color.red : Color.black;

        int voiceLength = 0;

        if (content.Length > 80)
        {
            voiceLength = 2;
        }
        else if (content.Length > 50)
        {
            voiceLength = 1;
        }

        AudioManager.instance.PlayClippyVoice(state == State.EVIL, voiceLength);
    }

    public void ChangePos(Vector2 newPos)
    {
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = newPos;
    }

    public void Show(float duration)
    {
        shown = true;
        this.duration = duration;
        visual.SetActive(true);
        timeShown = Time.time;
    }

    public void Hide()
    {
        shown = false;
        visual.SetActive(false);
    }

    private void Start()
    {
        state = State.NORMAL;
        Instance = this;
        ShowSave(false);
        ShowDefense(false);
    }

    private void Update()
    {
        Animate();

        if (shown)
        {
            if (duration > 0)
            {
                if (Time.time - timeShown > duration)
                {
                    shown = false;
                    visual.SetActive(false);
                }
            }
        }
    }

    private Sprite GetStateSprite()
    {
        switch (state)
        {
            case State.NORMAL:
                return normalClippy;
            case State.TROUBLED:
                return troubledClippy;
            case State.BAD:
                return badClippy;
            case State.EVIL:
                return evilClippy;
            default:
                return normalClippy;
        }
    }

    private void Animate()
    {
        Vector2 pos = refTransform.gameObject.GetComponent<RectTransform>().anchoredPosition;
        imageTransform.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, pos.y + Mathf.Sin(Time.time * 2) * 5);
    }
}
