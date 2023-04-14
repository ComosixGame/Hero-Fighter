using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private SoundManager soundManager;
    public AudioClip fallSound;
    public AudioClip attackSound;
    public AudioClip dieSound;
    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    public void PlayEnemySound(int index){
        switch(index){
            case 1: 
                soundManager.PlaySound(fallSound);
                break;
            case 2:
                soundManager.PlaySound(attackSound);
                break;
            case 3:
                 soundManager.PlaySound(dieSound);
                break;
            default:
                break;
        }
    }
}
