using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    private void OnEnable()
    {
        UIHandler.OnStartsdfEvent += Startsdf;
    }

    private void OnDisable()
    {
        UIHandler.OnStartsdfEvent -= Startsdf;
    }

    private void Startsdf()
    {
        StartCoroutine(StartVictoryGlitch());
    }

    private IEnumerator StartVictoryGlitch()
    {
        GlitchEffect.instance.SetGlitch(0);

        yield return new WaitForSeconds(2);

        GlitchEffect.instance.SetGlitch(1, 2);
        AudioManager.instance.PlayGlitch();

        yield return new WaitForSeconds(5);

        GlitchEffect.instance.SetGlitch(1, 2);
        AudioManager.instance.PlayGlitch();
    }
}
