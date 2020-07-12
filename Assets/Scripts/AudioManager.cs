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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayClickSound();
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

    public void PlayProgressBarBreak()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/progressbar");
        PlaySFX(clip);
    }

    public void PlayAmbiance(AudioClip clip, float volume = 1f)
    {
        if (ambiance == null)
        {
            ambiance = CreateNewSource(string.Format("AMBIANCE [{0}]", clip.name));
            DontDestroyOnLoad(ambiance);
        }
        ambiance.clip = clip;
        ambiance.Play();
        ambiance.loop = true;
        ambiance.volume = volume;
    }

    public void StopAmbiance()
    {
        if (ambiance != null)
        {
            ambiance.Stop();
        }
    }

    public void PlayWordGameAmbiance()
    {
        PlayAmbiance(Resources.Load<AudioClip>("Ambiance/letters-music"));
    }
    
    public void PlayMazeMusic()
    {
        PlayAmbiance(Resources.Load<AudioClip>("Ambiance/maze-music"), 0.5f);
    }

    public void PlayClickSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/mouse-click/click" + Random.Range(1, 3));
        PlaySFX(clip, 0.5f);
    }

    /// <summary>
    /// Play Clippy voice
    /// </summary>
    /// <param name="isEvil"></param>
    /// <param name="length">0 = short, 1 = medium, 2 = long</param>
    public void PlayClippyVoice(bool isEvil, int lengthType = 0)
    {
        string length = "medium";
        string evil = isEvil ? "evil" : "normal";
        switch(lengthType)
        {
            case 0:
                length = "short";
                break;
            case 1:
                length = "medium";
                break;
            case 2:
                length = "long";
                break;
        }

        AudioClip clip = Resources.Load<AudioClip>("Sounds/clippy/" + evil + "_" + length);

        if (isEvil)
        {
            PlaySFX(clip);
        }
        else
        {
            PlaySFX(clip, 0.4f);
        }
    }

    public void PlayPickupLetter(bool goodLetter)
    {
        string path = goodLetter ? "good" : "bad";
        AudioClip clip = Resources.Load<AudioClip>("Sounds/word-game/" + path);
        PlaySFX(clip);
    }

    public void PlayCollisionSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/maze/collision");
        PlaySFX(clip);
    }

    public void PlayIntroAmbiance()
    {
        PlayAmbiance(Resources.Load<AudioClip>("Ambiance/intro"));
    }

    public void PlayStartSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/start/startup");
        PlaySFX(clip);
    }

    public void PlayGameOverSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/end/gameover");
        PlaySFX(clip);
    }

    public void PlayVictorySound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/end/victory");
        PlaySFX(clip);
    }
}


