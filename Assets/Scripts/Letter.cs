using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    private string letter;
    private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public TextMeshPro text;

    private void Start()
    {
        GenerateLetter();
    }

    private void GenerateLetter()
    {
        string letters = WordGameHandler.instance.GetLetters();
        letter = letters[Random.Range(0, letters.Length)].ToString();
        text.text = letter;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Palette")
        {
            WordGameHandler.instance.AddLetter(letter);
        }
        Destroy(gameObject);
    }
}
