using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Slider bgmSlider; 
    public Slider sfxSlider; 

    private const string BGM = "BGM Volume";
    private const string SFX = "SFX Volume";

    private const float MIN_VOLUME_DB = -80f;

    private void Start()
    {
        
        bgmSlider.value = PlayerPrefs.GetFloat(BGM, 0.75f); 
        sfxSlider.value = PlayerPrefs.GetFloat(SFX, 0.75f);

        
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);

        
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        if (volume <= 0.001f) 
        {
            audioMixer.SetFloat(BGM, MIN_VOLUME_DB); 
        }
        else
        {
            audioMixer.SetFloat(BGM, Mathf.Log10(volume) * 20); 
        }

        PlayerPrefs.SetFloat(BGM, volume); 
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.001f) 
        {
            audioMixer.SetFloat(SFX, MIN_VOLUME_DB); 
        }
        else
        {
            audioMixer.SetFloat(SFX, Mathf.Log10(volume) * 20); 
        }

        PlayerPrefs.SetFloat(SFX, volume); 
    }
}
