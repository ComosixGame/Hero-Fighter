using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    public event Action<bool> OnMute;
    private bool isMute;
    private SettingData settingData;

    protected override void Awake()
    {
        base.Awake();
        settingData = SettingData.Load();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.mute = isMute;
    }


    public void PlayOneShot(AudioClip audioClip, float volumeScale = 1)
    {
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public AudioSource AddAudioSource(GameObject parent)
    {
        AudioSource audioSource = parent.AddComponent<AudioSource>();
        audioSource.mute = isMute;
        return audioSource;
    }

    public void MuteGame(bool mute)
    {
        settingData.mute = mute;
        settingData.Save();
        audioSource.mute = mute;
        OnMute?.Invoke(mute);
        
    }
}
