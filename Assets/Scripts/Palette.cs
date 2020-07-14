using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    private bool cursorLocked;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
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
            Vector3 offset = new Vector3(Input.GetAxis("Mouse X"), 0, 0) * Parameters.MouseSensitivity;
            bool movementOk = true;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, offset, offset.magnitude);
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Wall")
                {
                    movementOk = false;
                    break;
                }
            }
            
            if (movementOk)
            {
                transform.Translate(offset);
            }
        }
    }
}
