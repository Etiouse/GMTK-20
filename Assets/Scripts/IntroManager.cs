using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> texts;

    private int currentTextIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in texts)
        {
            item.SetActive(false);
        }
        texts[0].SetActive(true);

        AudioManager.instance.PlayIntroAmbiance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            texts[currentTextIndex].SetActive(false);

            currentTextIndex++;

            if (currentTextIndex < texts.Count)
            {
                texts[currentTextIndex].SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}
