using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip clickButton;
    [SerializeField] private AudioClip selectChapter;
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    public void PlaySoundEffect(int index)
    {
        switch (index)
        {
            case 1:
                soundManager.PlaySound(clickButton);
                break;
            case 2:
                soundManager.PlaySound(selectChapter);
                break;
            default:
                break;
        }
    }
}
