using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordGameHandler : MonoBehaviour
{
    public float delayBetweenLetters = 0.3f;
    public Transform MinSpawn;
    public Transform MaxSpawn;
    public GameObject LetterPrefab;

    public List<string> Words;
    public TextMeshProUGUI CurrentText;
    public GameObject WordList;
    public GameObject WordPrefab;

    private string currentText;
    private string currentWord;
    private int currentIndex = 0;

    public static WordGameHandler instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine(SpawnLetter());
        foreach(string word in Words)
        {
            GameObject w = Instantiate(WordPrefab, WordList.transform);
            w.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = word;
        }
        currentWord = Words[0];
        WordList.transform.GetChild(currentIndex).Find("Icon_current").GetComponent<CanvasGroup>().alpha = 1;
        AudioManager.instance.PlayAmbiance(Resources.Load<AudioClip>("Ambiance/letters-music"));
    }

   
    IEnumerator SpawnLetter()
    {
        yield return new WaitForSeconds(delayBetweenLetters);

        float x = Random.Range(MinSpawn.position.x, MaxSpawn.position.x);
        float y = MinSpawn.position.y;

        Vector3 pos = new Vector3(x, y, 0);
        
        Instantiate(LetterPrefab, pos, Quaternion.identity);
        StartCoroutine(SpawnLetter());
    }

    public void AddLetter(string letter)
    {
        
        currentText += letter;
        if (!currentWord.ToLower().StartsWith(currentText.ToLower()))
            currentText = "";
        CurrentText.text = currentText;

        if (currentText.ToLower() == currentWord.ToLower())
        {
            NextWord();
        }
    }

    private void NextWord()
    {
        currentText = "";
        CurrentText.text = "";
        WordList.transform.GetChild(currentIndex).Find("Icon_ok").gameObject.SetActive(true);
        WordList.transform.GetChild(currentIndex).Find("Icon_pending").gameObject.SetActive(false);
        WordList.transform.GetChild(currentIndex).Find("Icon_current").GetComponent<CanvasGroup>().alpha = 0;
        currentIndex++;

        if (currentIndex < Words.Count)
        {
            currentWord = Words[currentIndex];
            WordList.transform.GetChild(currentIndex).Find("Icon_current").GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            print("WIN");
        }


    }

    public string GetLetters()
    {
        return currentWord;
    }
}
