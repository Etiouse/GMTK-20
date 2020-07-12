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

    public static Clippy Instance;

    public enum State { NORMAL, TROUBLED, BAD, EVIL };
    private State state;
    private Vector3 pos;

    private bool shown;
    private float duration;
    private float timeShown;

    public void ChangeState(State newState)
    {
        state = newState;

        clippy.overrideSprite = GetStateSprite();
    }

    public void ChangeText(string content, bool showButtons)
    {
        message.text = content;

        acceptButton.gameObject.SetActive(showButtons);
        rejectButton.gameObject.SetActive(showButtons);

        message.color = state == State.EVIL ? Color.red : Color.black;
    }

    public void ChangePos(Vector2 newPos)
    {
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = newPos;
        pos = new Vector3(imageTransform.position.x, imageTransform.position.y, imageTransform.position.z);
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
        pos = new Vector3(imageTransform.position.x, imageTransform.position.y, imageTransform.position.z);
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
        imageTransform.position = new Vector3(pos.x, pos.y + Mathf.Sin(Time.time * 2) * 5, pos.z);
    }
}
