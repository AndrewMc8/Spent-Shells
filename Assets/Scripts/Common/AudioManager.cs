using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] GameData gameData;

    const string MASTER_VOLUME = "MasterVolume";
    const string SFX_VOLUME = "SFXVolume";
    const string MUSIC_VOLUME = "MusicVolume";

    public float masterVolume
    {
        get
        {
            audioMixer.GetFloat(MASTER_VOLUME, out float dB);
            return DBToLinear(dB);
        }
        set
        {
            float dB = LinearToDB(value);
            audioMixer.SetFloat(MASTER_VOLUME, dB);

            gameData.floatData[MASTER_VOLUME] = value;
        }
    }



    public float sfxVolume
    {
        get
        {
            audioMixer.GetFloat(SFX_VOLUME, out float dB);
            return DBToLinear(dB);
        }
        set
        {
            float dB = LinearToDB(value);
            audioMixer.SetFloat(SFX_VOLUME, dB);
            gameData.floatData[SFX_VOLUME] = value;
        }
    }



    public float musicVolume
    {
        get
        {
            audioMixer.GetFloat(MUSIC_VOLUME, out float dB);
            return DBToLinear(dB);
        }
        set
        {
            float dB = LinearToDB(value);
            audioMixer.SetFloat(MUSIC_VOLUME, dB);
            gameData.floatData[MUSIC_VOLUME] = value;
        }
    }

    void Start()
    {
        masterVolume = gameData.floatData[MASTER_VOLUME];
        sfxVolume = gameData.floatData[SFX_VOLUME];
        musicVolume = gameData.floatData[MUSIC_VOLUME];
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public static float LinearToDB(float linear)
    {
        return (linear != 0) ? 20.0f * Mathf.Log10(linear) : -144.0f;
    }

    public static float DBToLinear(float dB)
    {
        return Mathf.Pow(10.0f, dB / 20.0f);
    }
}

