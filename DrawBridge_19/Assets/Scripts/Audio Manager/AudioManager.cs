using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    Sound bgm;
    bool playedBgm = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

        }
    }

    private void Start()
    {
        //Play("Background");

        bgm = Array.Find(sounds, sound => sound.name.Equals("Background"));
        if (bgm == null)
            Debug.LogWarning(bgm.name + "sound does not exist");
        bgm.source.Play();
    }

    private void Update()
    {
        if (Controller.instance.bgmOn)
            if (!playedBgm)
            {
                bgm.source.UnPause();
                playedBgm = true;
            }
        if (!Controller.instance.bgmOn)
            if (playedBgm)
            {
                bgm.source.Pause();
                playedBgm = false;
            }
    }

    public void Play(string name)
    {
        if (!Controller.instance.audioOn)
            return;

        Sound s = Array.Find(sounds, sound => sound.name.Equals(name));
        if (s == null)
            Debug.LogWarning(s.name + "sound does not exist");
        s.source.Play();
    }
}
