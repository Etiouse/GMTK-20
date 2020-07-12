using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMazeManager : MonoBehaviour
{
    [SerializeField]
    private float mouseSpeed = 5f;

    private bool cursorLocked;

    private bool isBallOutOfPath;

    private bool hasApplicationFocus;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
        isBallOutOfPath = false;
    }

    void Update()
    {
        if (isBallOutOfPath)
        {
            isBallOutOfPath = false;
            SetBallPosition(MazeHandler.GetCurrentSpawn().transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = false;
        }

        if (!cursorLocked && (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)))
        {
            cursorLocked = true;
        }

        if (cursorLocked)
        {
            Vector3 offset = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSpeed;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, offset, offset.sqrMagnitude);
            Debug.DrawRay(transform.position, offset, Color.red, 5);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Wall")
                {
                    Debug.Log(hit.collider.name);
                    isBallOutOfPath = true;
                    break;
                }
            }

            if (!isBallOutOfPath)
            {
                MoveBall(offset * mouseSpeed);
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void MoveBall(Vector3 offset)
    {
        transform.position += offset;
    }

    public void SetBallPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spawn")
        {
            int newSpawnIndex = collision.GetComponent<SpawnStorage>().SpawnIndex;
            Debug.Log(newSpawnIndex + " " + MazeHandler.CurrentSpawnIndex);

            if (newSpawnIndex > MazeHandler.CurrentSpawnIndex)
            {
                MazeHandler.CurrentSpawnIndex = newSpawnIndex;

                if (newSpawnIndex >= MazeHandler.GetMaxSpawnNumber() - 1)
                {
                    Debug.Log("Win");
                }
                else
                {
                    MazeHandler.HighlightNextSpawn();
                }
            }
        }
    }
}
