using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum Audio
{
    footstep,
    attack,
    teleport,
    dodge,
    interact,
    dialogue
}

public class SoundManager : Singleton<SoundManager>
{
    
    [SerializeField] private AudioSource[] _audioList;

    public Dictionary<Audio, AudioSource> AudioDic;
    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        AudioDic = new Dictionary<Audio, AudioSource>();
    }
    private void Start()
    {
        foreach (Audio value in Enum.GetValues(typeof(Audio)))
        {
            AudioDic.Add(value, _audioList[(int)value]);
        }
        
    }
    public void PlaySoundOnce(Audio audio)
    {
        AudioDic[audio].PlayOneShot(AudioDic[audio].clip);
    }

    public void PlaySoundOnce(Audio audio, float volume)
    {
        AudioDic[audio].PlayOneShot(AudioDic[audio].clip, volume);
    }

    public void PlaySoundLooped(Audio audio)
    {
        AudioDic[audio].loop = true;
        AudioDic[audio].Play(0);
    }

    public void PlaySoundLooped(Audio audio, float volume)
    {
        AudioDic[audio].loop = true;
        AudioDic[audio].volume = volume;
        AudioDic[audio].Play();
    }
    public void StopSound(Audio audio)
    {
        if (AudioDic[audio].isPlaying)
        {
            AudioDic[audio].Stop();
        }
    }

}
