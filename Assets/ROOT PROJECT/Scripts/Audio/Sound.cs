using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;

    public string name;

    [Range(0f, 10f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;


}
