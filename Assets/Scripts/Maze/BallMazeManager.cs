﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallMazeManager : MonoBehaviour
{
    private bool cursorLocked;

    private bool isBallOutOfPath;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
        isBallOutOfPath = false;
        GlitchEffect.instance.SetGlitch(1, 1, 0.1f);
    }

    void Update()
    {
        if (isBallOutOfPath)
        {
            isBallOutOfPath = false;
            SetBallPosition(MazeHandler.GetCurrentSpawn().transform.position);

            AudioManager.instance.PlayCollisionSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = false;
        }

        if (!cursorLocked && (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)))
        {
            cursorLocked = true;
        }

        if (cursorLocked &&
            !Parameters.IsOpened)
        {
            Cursor.lockState = CursorLockMode.Locked;

            Vector3 offset = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * Parameters.MouseSensitivity;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, offset, offset.sqrMagnitude);
            //Debug.DrawRay(transform.position, offset, Color.red, 5);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Wall")
                {
                    isBallOutOfPath = true;
                    break;
                }
            }

            if (!isBallOutOfPath)
            {
                MoveBall(offset * Parameters.MouseSensitivity);
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

            if (newSpawnIndex > MazeHandler.CurrentSpawnIndex)
            {
                MazeHandler.CurrentSpawnIndex = newSpawnIndex;

                if (newSpawnIndex < MazeHandler.GetMaxSpawnNumber() - 1)
                {
                    MazeHandler.HighlightNextSpawn();
                }
            }
        }
        else if (collision.tag == "ButtonAdminConfirm")
        {
            MazeHandler.CurrentButton = MazeHandler.Button.ADMIN_CONFIRM;
        }
        else if (collision.tag == "ButtonProgressDone")
        {
            MazeHandler.CurrentButton = MazeHandler.Button.PROGRESS_DONE;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ButtonAdminConfirm" ||
            collision.tag == "ButtonProgressDone")
        {
            MazeHandler.CurrentButton = MazeHandler.Button.NONE;
        }
    }
}
