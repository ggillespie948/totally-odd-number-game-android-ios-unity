using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance = null;

	// Use this for initialization
	void Awake () {

        instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
		
	}
	
	public void Play(string name)
    {
        if (name == null)
            return;


        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

    }
}
