using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum Audio
{
    #region player
    footstep,
    attack,
    teleportIn,
    teleportOut,
    dodge,
    interact,
    dialogue,
    #endregion
    #region background
    wind,
    townMusic,
    mobFightMusic,
    bossBattleMusic,
    #endregion
    None
}

public class SoundManager : Singleton<SoundManager>
{
    
    [SerializeField] private AudioSource[] _audioList;

    public Dictionary<Audio, AudioSource> AudioDic;
    AudioSource audioSource;

    public Audio CurrBackgroundMusic;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        AudioDic = new Dictionary<Audio, AudioSource>();
    }
    private void Start()
    {
        var values = Enum.GetValues(typeof(Audio)).Cast<Audio>().ToArray();

        foreach (Audio value in values.Take(values.Length - 1))
        {
            AudioDic.Add(value, _audioList[(int)value]);
        }

        PlayBgSoundLooped(Audio.wind);


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
    public void PlayBgSoundLooped(Audio audio)
    {
        CurrBackgroundMusic = audio;
        AudioDic[audio].loop = true;
        AudioDic[audio].Play(0);
    }

    public void StopBgSound()
    {
        AudioDic[CurrBackgroundMusic].Stop();
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
