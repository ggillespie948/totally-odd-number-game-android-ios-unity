using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

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

        if(!ApplicationModel.MUSIC_ENABLED)
            return;

        Sound t = Array.Find(sounds, sound => sound.name == "title");
        t.source.Play();
		
	}

    public void ToggleSoundFX()
    {
        ApplicationModel.FX_ENABLED = !ApplicationModel.FX_ENABLED;
    }

    public void ToggleMusic()
    {
        ApplicationModel.MUSIC_ENABLED = !ApplicationModel.MUSIC_ENABLED;

        if(ApplicationModel.MUSIC_ENABLED)
        {
            Sound t = Array.Find(sounds, sound => sound.name == "title");
            t.source.Play();
        } else
        {
            Sound t = Array.Find(sounds, sound => sound.name == "title");
            t.source.Stop();
        }
    }
	
	public void Play(string name)
    {
        if(!ApplicationModel.FX_ENABLED)
            return;

        if (name == null)
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s!= null)
            s.source.Play();

    }

    public void PlayAfterDelay(float delay, string name)
    {
        StartCoroutine(PlayDelay(delay, name));
    }

    private IEnumerator PlayDelay(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        Play(name);
    } 
}
