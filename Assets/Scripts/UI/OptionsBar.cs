using UnityEngine;
using UnityEngine.UI;

public class OptionsBar : MonoBehaviour
{
    [SerializeField] private GameObject fileBarObject = null;
    [SerializeField] private GameObject homeBarObject = null;

    [SerializeField] private Button fileButton = null;
    [SerializeField] private Button homeButton = null;

    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color unselectedColor = Color.black;


    public void ShowFileBar()
    {
        homeBarObject.SetActive(false);
        homeButton.GetComponent<Image>().color = unselectedColor;
        fileBarObject.SetActive(true);
        fileButton.GetComponent<Image>().color = selectedColor;
    }

    public void ShowHomeBar()
    {
        fileBarObject.SetActive(false);
        fileButton.GetComponent<Image>().color = unselectedColor;
        homeBarObject.SetActive(true);
        homeButton.GetComponent<Image>().color = selectedColor;
    }

    private void Start()
    {
        ShowHomeBar();
    }
}
