using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range (0.1f, 3f)]
    public float pitch = 1;

    public bool isLooping = false;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    [SerializeField] private List<Sound> _sounds = new List<Sound>();

    [Header("Flow sounds")]
    [SerializeField] private string _winSound = "";
    [SerializeField] private string _looseSound = "";

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        PopulateAudioSources();
    }

    private void PopulateAudioSources()
    {
        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.isLooping;
        }
    }

    public void Play(string name)
    {
        Sound lSound = _sounds.Find(sound => sound.name == name);

        if (lSound != null)
            lSound.source.Play();
    }

    public void PlayWinSound()
    {
        Play(_winSound);
    }

    public void PlayLooseSound()
    {
        Play(_looseSound);
    }

    private void OnDestroy()
    {
        if(_instance == this)
            _instance = null;
    }


}
