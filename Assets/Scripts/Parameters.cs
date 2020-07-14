using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{
    [SerializeField]
    private GameObject parametersCanvasWrapper;

    [SerializeField]
    private TMP_Text sensitivityText;

    [SerializeField]
    private Slider sensitivitySlider;

    public static float MouseSensitivity { get; private set; }

    public static bool IsOpened = false;

    private void Start()
    {
        parametersCanvasWrapper.SetActive(false);
        SetSensitivity(sensitivitySlider.value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.P) ||
            Input.GetKeyDown(KeyCode.S))
        {
            OpenCloseParameters(!IsOpened);
        }

        if (IsOpened)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void OpenCloseParameters(bool isOpening)
    {
        parametersCanvasWrapper.SetActive(isOpening);
        IsOpened = isOpening;
    }

    public void SetSensitivity(float value)
    {
        MouseSensitivity = value;
        sensitivityText.text = (((int)(value * 1000)) / 1000.0).ToString();
    }
}
