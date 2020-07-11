using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public float MouseSpeed = 0.2f;

    public CursorHandler instance;

    private bool cursorLocked;
    private BoxCollider2D collider;


    private void Start()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
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
            Vector3 offset = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
           

            RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, offset.sqrMagnitude, LayerMask.NameToLayer("Cursor"));
            Debug.DrawRay(transform.position, offset, Color.red, 5);

            // If it hits something...
            if (hit.collider != null)
            {
                ResetToCheckPoint();
            }
            else
            {
                transform.Translate(offset * MouseSpeed);
            }
        }
        
    }

    public void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ResetToCheckPoint();
        }
    }

    private void ResetToCheckPoint()
    {
        MoveTo(Vector3.zero);
    }
}
