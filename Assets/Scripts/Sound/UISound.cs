using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    private SoundManager soundManager;

    public AudioClip soundUI;
    public AudioClip soundBackground;

    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    private void Start() {
        soundManager.SetMusicbackGround(soundBackground);
        soundManager.SetPlayMusic(true);
    }

    public void PlaySoundUI(){
        soundManager.PlaySound(soundUI);
    }
}
