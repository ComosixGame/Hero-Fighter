using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private SoundManager soundManager;
    public AudioClip punchingOneSound;
    public AudioClip punchingTwoSound;
    public AudioClip mmaKickOneSound;
    public AudioClip mmaKichTwoSound; 
    public AudioClip walkSound;
    public AudioClip dieSound;

    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    public void PlaySoundGame(int index){
        switch(index){
            case 1: 
                soundManager.PlaySound(punchingOneSound);
                break;
            case 2:
                soundManager.PlaySound(punchingTwoSound);
                break;
            case 3:
                soundManager.PlaySound(mmaKickOneSound);
                break;
            case 4:
                soundManager.PlaySound(mmaKichTwoSound);
                break;
            case 5:
                soundManager.PlaySound(walkSound);
                break;
            case 6: 
                soundManager.PlaySound(dieSound);
                break;
            default:
                break;
        }
    }
}
