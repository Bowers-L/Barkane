using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private Sound[] sounds;
    private static Sound[] _sounds;
    [SerializeField]
    //private Sound[] music;
    //private static Sound[] _music;
    public ArrayClipArray[] musicLists;

    private static float sfxVolume = 0.5f; // [0..1]
    private static float musicVolume = 0.5f;

    //[SerializeField]
    //private GameObject audioListenerObj;
    //private static AudioLowPassFilter menuLowPass;

    //private static string currentMusic;
    //public static bool isPlaying;
    private int currentArray;
    private int currentIndex;
    private AudioSource source;


    void Awake()
    {
        InitializeSingleton(this.gameObject);
        DontDestroyOnLoad(gameObject);

        _sounds = sounds;

        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch + 1f;
            s.source.loop = s.loop;
        }
        source = GetComponent<AudioSource>();



        //menuLowPass = audioListenerObj.GetComponent<AudioLowPassFilter>();
        SetSFXVolume(sfxVolume * 100);
        SetMusicVolume(musicVolume * 100);
    }

    private void Start()
    {
        PlayMusic();
    }

    public void Play(string name)
    {
        if (_sounds == null)
            return;
        Sound s = Array.Find(_sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        if(s.source == null)
            return;
        if (s.doRandomPitch)
            s.source.pitch = s.pitch * UnityEngine.Random.Range(.95f, 1.05f);

        s.source.Play();
    }

    public void PlayMusic(int list, int ind)
    {
        source.Stop();
        if (musicLists == null)
            return;
        AudioClip s = musicLists[list].Tracks[ind];

        if(ind > musicLists[list].Tracks.Length)
            ind = 0;

        if (s == null)
        {
            Debug.LogError("Music track does not exist!");
            return;
        }
        source.clip = s;
        source.Play();
    }
    public void PlayMusic() {
        PlayMusic(currentArray, currentIndex);
    }

    public void PlayList(string name) {
        int curr = -1;
        if (musicLists == null)
            return;
        for (int i = 0; i < musicLists.Length; i++) {
            if (musicLists[i].name == name) {
                curr = i;
            }
        }
        if (curr == -1) {
            Debug.LogError("Music list" + name + " could not be found.");
            return;
        } else {
            currentArray = curr;
            PlayMusic(currentArray, 0);
        }
    }

    void Update() {
        if (!source.isPlaying) {
            currentIndex++;
            if (currentIndex >= musicLists[currentArray].Tracks.Length) {
                currentIndex = 0;
            }
            PlayMusic(currentArray, currentIndex);
        }
    }

    public static void Stop(string name)
    {
        if (_sounds == null)
            return;
        Sound s = Array.Find(_sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }


    public static void SetSFXVolume(float value)
    {
        value = (Mathf.Clamp(value, 0, 100)/100.0f);
        sfxVolume = value;

        if (_sounds == null)
            return;
        foreach (Sound s in _sounds)
        {
            if (s == null || s.source == null)
                continue;
            s.source.volume = s.volume * value;
        }
    }

    public static void SetMusicVolume(float value)
    {
        value = (Mathf.Clamp(value, 0, 100)/100.0f);
        musicVolume = value;
        Instance.source.volume = musicVolume;
    }

    public static void SetPitch(float value)
    {
        value = Mathf.Clamp(value, 0.3f, 3f);
        //volume = value;

        if (_sounds == null)
            return;
        foreach (Sound s in _sounds)
        {
            if (s == null || s.source == null)
                continue;
            s.source.pitch = s.pitch * value;
        }
    }

    public static float GetSFXVolume()
    {
        return sfxVolume;
    }

    public static float GetMusicVolume()
    {
        return musicVolume;
    }

    //public static void SetLowPassEnabled(bool value)
    //{
    //    menuLowPass.enabled = value;
    //}
}