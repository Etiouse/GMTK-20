using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotateManager : MonoBehaviour
{
    [SerializeField]
    private bool isRotatingToRight = true;

    [SerializeField]
    private float rotationAngle = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotatingToRight)
        {
            transform.Rotate(Vector3.back, rotationAngle * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward, rotationAngle * Time.deltaTime);
        }
    }
}
