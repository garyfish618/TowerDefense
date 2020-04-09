using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public PersistenceController contr;

    void Awake() {
        contr = PersistenceController.Instance;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
        }

    }

    public void Play(string name) {

        //If sound is muted ignore
        if(!contr.SoundAudible)
        {
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        s.source.Play();
    }

}
