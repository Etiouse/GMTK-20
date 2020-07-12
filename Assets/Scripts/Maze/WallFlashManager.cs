using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFlashManager : MonoBehaviour
{
    [SerializeField]
    private float flashDuration = 2;

    [SerializeField]
    private float safeDuration = 2;

    [SerializeField]
    private float startingTime = 0;

    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = startingTime;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= flashDuration + safeDuration)
        {
            elapsedTime = 0;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (elapsedTime >= safeDuration)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
