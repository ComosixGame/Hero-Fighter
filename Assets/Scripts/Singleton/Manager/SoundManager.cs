using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    public UnityEvent<bool> OnMute = new UnityEvent<bool>();
    private GameManager gameManager;
    private bool isMute;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        // isMute = gameManager.settingData.mute;
        audioSource.mute = isMute;
    }


    public void PlayOneShot(AudioClip audioClip, float volumeScale = 1) {
        audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public AudioSource AddAudioSource(GameObject parent) {
        AudioSource audioSource = parent.AddComponent<AudioSource>();
        audioSource.mute = isMute;
        return audioSource;
    }

    public void MuteGame(bool mute) {
        isMute =  mute;
        audioSource.mute = mute;
        // gameManager.settingData.mute = mute;
        // gameManager.settingData.Save();
        OnMute?.Invoke(mute);
    }
}
