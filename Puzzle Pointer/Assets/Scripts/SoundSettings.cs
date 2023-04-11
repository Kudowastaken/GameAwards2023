using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : SingletonPersistent<SoundSettings>
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixer SFXMixer;
    [SerializeField] Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        SetSFX(PlayerPrefs.GetFloat("SavedSFXVolume", 100));
    }

    public void SetVolume(float _value)
    {
        if (_value < 1) {
            _value = .001f;
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetSFX(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        RefreshSFXSlider(_value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        SFXMixer.SetFloat("SoundEffectVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetVolumeFromSlider(){
        SetVolume(soundSlider.value);
    }

    public void SetSFXFromSlider()
    {
        SetSFX(SFXSlider.value);
    }

    public void RefreshSlider(float _value){
        soundSlider.value = _value;
    }

    public void RefreshSFXSlider(float _value)
    {
        SFXSlider.value = _value;
    }
}
