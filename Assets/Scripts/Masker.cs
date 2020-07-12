using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masker : MonoBehaviour
{

    public float TotalDisplayTime = 5;
    List<GameObject> masks = new List<GameObject>();
    float totalSizes = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalSizes = 0;
        foreach (Transform child in transform)
        {
            masks.Add(child.gameObject);
            totalSizes += child.localScale.x;
        }
        StartReveal();
    }

    private void StartReveal()
    {
        StartCoroutine(Reveal(masks[0], 0));
    }

    IEnumerator Reveal(GameObject mask, int index)
    {
        Vector3 originalScale = mask.transform.localScale;
        Vector3 destinationScale = new Vector3(0f, originalScale.y, originalScale.z);

        float time = mask.transform.localScale.x / totalSizes * TotalDisplayTime;

        float currentTime = 0.0f;

        do
        {
            mask.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        mask.transform.localScale = destinationScale;

        index++;

        if (index < masks.Count)
        {
            StartCoroutine(Reveal(masks[index], index));
        }
    }
}
