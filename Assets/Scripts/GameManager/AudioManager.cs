using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private const string BGM = "BGM Volume";
    private const string SFX = "SFX Volume";
    private const float MIN_VOLUME_DB = -80f;

    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(BGM, 0.50f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX, 0.75f);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(BGM, volume <= 0.001f ? MIN_VOLUME_DB : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(BGM, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX, volume <= 0.001f ? MIN_VOLUME_DB : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX, volume);
    }

    public void PlayScoreSound()
    {
        // Play score-related sound
    }

    public void PlayStartSound()
    {
        // Play start sound
    }

    public void PlayGameOverSound()
    {
        // Play game over sound
    }
}
