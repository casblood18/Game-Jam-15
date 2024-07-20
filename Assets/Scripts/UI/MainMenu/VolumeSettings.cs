using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    private const string VOLUMEPREFS = "Volume";

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        LoadVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(VOLUMEPREFS, volumeSlider.value);
    }

    public void SetVolume(float volume)
    {
        volumeSlider.value = volume;
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    private void LoadVolume()
    {
        if (!PlayerPrefs.HasKey(VOLUMEPREFS))
        {
            SetDefaultValues();
            return;
        }

        float volume = PlayerPrefs.GetFloat(VOLUMEPREFS);
        SetVolume(volume);
    }

    private void SetDefaultValues()
    {
        SetVolume(0.3f);
    }
}
