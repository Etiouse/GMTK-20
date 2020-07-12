using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMoveLineManager : MonoBehaviour
{
    private enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    [SerializeField]
    private Direction currentDirection = Direction.UP;

    [SerializeField]
    private float movementSpeed = 1;

    private bool canDirectionBeInversed = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentDirection)
        {
            case Direction.UP:
                transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
                break;
            case Direction.DOWN:
                transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
                break;
            case Direction.RIGHT:
                transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
                break;
            case Direction.LEFT:
                transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WallInversor" &&
            canDirectionBeInversed)
        {
            canDirectionBeInversed = false;
            InverseDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WallInversor")
        {
            canDirectionBeInversed = true;
        }
    }

    private void InverseDirection()
    {
        switch (currentDirection)
        {
            case Direction.UP:
                currentDirection = Direction.DOWN;
                break;
            case Direction.DOWN:
                currentDirection = Direction.UP;
                break;
            case Direction.RIGHT:
                currentDirection = Direction.LEFT;
                break;
            case Direction.LEFT:
                currentDirection = Direction.RIGHT;
                break;
        }
    }
}
