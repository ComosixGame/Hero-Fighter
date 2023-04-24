using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip knockDownSound;
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private AudioClip walkSound;

    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    public void PlaySound(int index){
        switch(index){
            case 1: 
                soundManager.PlaySound(hitSound);
                break;
            case 2:
                soundManager.PlaySound(knockDownSound);
                break;
            case 3:
                soundManager.PlaySound(deadSound);
                break;
            default:
                break;
        }
    }
}
