using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource ambiance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySFX(AudioClip effect, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = CreateNewSource(string.Format("SFX [{0}]", effect.name));
        MakeVolatileChild(source.transform);
        source.clip = effect;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, effect.length);
    }

    public static AudioSource CreateNewSource(string _name)
    {
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource;
    }

    GameObject volatileParent = null;
    void MakeVolatileChild(Transform ob)
    {
        if (volatileParent == null)
            volatileParent = new GameObject("[AUDIOMANAGER VOLATILE]");

        ob.parent = volatileParent.transform;
    }

    public void PlayGlitch()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/glitch/glitch" + Random.Range(1, 6));
        PlaySFX(clip);
    }

    public void PlayAmbiance(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (ambiance == null)
        {
            ambiance = CreateNewSource(string.Format("AMBIANCE [{0}]", clip.name));
            MakeVolatileChild(ambiance.transform);
        }
        ambiance.clip = clip;
        ambiance.Play();
        ambiance.loop = true;
    }

    public void StopAmbiance()
    {
        if (ambiance != null)
        {
            ambiance.Stop();
        }
    }
}


