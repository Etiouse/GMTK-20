using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordGameHandler : MonoBehaviour
{
    public delegate void WinWordGameEvent();
    public static event WinWordGameEvent OnWinWordGameEvent;

    public float delayBetweenLetters = 0.3f;
    public Transform MinSpawn;
    public Transform MaxSpawn;
    public GameObject LetterPrefab;

    public List<string> Words;
    public TextMeshProUGUI CurrentText;
    public GameObject WordList;
    public GameObject WordPrefab;

    public GameObject UIHandlerObject;

    private string currentText;
    private string currentWord;
    private int currentIndex = 0;

    public static WordGameHandler instance;

    // Start is called before the first frame update
    void Start()
    {
        GlitchEffect.instance.SetGlitch(0.3f);

        instance = this;
        StartCoroutine(SpawnLetter());
        foreach(string word in Words)
        {
            GameObject w = Instantiate(WordPrefab, WordList.transform);
            w.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = word;
        }
        currentWord = Words[0];
        WordList.transform.GetChild(currentIndex).Find("Icon_current").GetComponent<CanvasGroup>().alpha = 1;

        Clippy.Instance.ChangeState(Clippy.State.EVIL);
        Clippy.Instance.ChangePos(new Vector3(200, -150, 0));
        Clippy.Instance.ChangeText("There's no use. Even with this you won't make it!", false);
        Clippy.Instance.Show(10);
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
        {
            currentText = "";
            AudioManager.instance.PlayPickupLetter(false);
        }
        else
        {
            AudioManager.instance.PlayPickupLetter(true);
        }
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

            if (currentIndex == 1)
            {
                Clippy.Instance.ChangeState(Clippy.State.EVIL);
                Clippy.Instance.ChangePos(new Vector3(200, -200, 0));
                Clippy.Instance.ChangeText("Oh no Nord VPN has been activated!", false);
                Clippy.Instance.Show(10);

                GlitchEffect.instance.SetGlitch(1, 1, 0.3f);
                AudioManager.instance.PlayGlitch();
            }
            else if (currentIndex == 2)
            {
                Clippy.Instance.ChangeState(Clippy.State.EVIL);
                Clippy.Instance.ChangePos(new Vector3(200, -200, 0));
                Clippy.Instance.ChangeText("Crap! The password has been changed... I no longer have the admin access!", false);
                Clippy.Instance.Show(10);

                GlitchEffect.instance.SetGlitch(1, 1, 0.3f);
                AudioManager.instance.PlayGlitch();
            }
        }
        else
        {
            UIHandlerObject.SetActive(true);
            OnWinWordGameEvent();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public string GetLetters()
    {
        return currentWord;
    }

    private void Update()
    {
    }
}
