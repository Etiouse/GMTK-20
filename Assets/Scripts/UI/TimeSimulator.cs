using UnityEngine;
using TMPro;

public class TimeSimulator : MonoBehaviour
{
    [SerializeField] private TMP_Text time = null;
    [SerializeField] private float timeTroubled = 1f;
    [SerializeField] private float minTimeBewtweenTroubles = 3f;
    [SerializeField] private float maxTimeBewtweenTroubles = 10f;

    public enum State { NORMAL, TROUBLED, CORRUPTED }
    private State state;

    private float timeTroubleStarted;
    private float lastTroubleTime;
    private bool troubled;
    private float nextTrouble;

    public void ChangeState(State newState)
    {
        state = newState;
    }

    private void Start()
    {
        troubled = true;
        ChangeState(State.NORMAL);
    }

    private void Update()
    {
        switch (state)
        {
            case State.NORMAL:
                time.text = CurrentTime();
                break;
            case State.TROUBLED:
                HandleTroubleState();
                time.text = troubled ? CorruptedTime() : CurrentTime();
                break;
            case State.CORRUPTED:
                time.text = CorruptedTime();
                break;
        }
    }

    private void HandleTroubleState()
    {
        if (troubled)
        {
            if (Time.time - timeTroubleStarted > timeTroubled)
            {
                troubled = false;
                nextTrouble = Random.Range(minTimeBewtweenTroubles, maxTimeBewtweenTroubles);
                lastTroubleTime = Time.time;
            }
        }
        else
        {
            if (Time.time - lastTroubleTime > nextTrouble)
            {
                troubled = true;
                timeTroubleStarted = Time.time;
            }
        }
    }

    private string CurrentTime()
    {
        return System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
    }

    private string CorruptedTime()
    {
        return Random.Range(0, 9) + "" + Random.Range(0, 9) + ":" + Random.Range(0, 9) + "" + Random.Range(0, 9);
    }
}
