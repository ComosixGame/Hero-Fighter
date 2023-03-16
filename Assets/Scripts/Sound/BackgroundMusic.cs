using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip audioWin, audioLose;
    private SoundManager soundManager;
    private GameManager gameManager;
    private AudioSource audioSource;

    private void Awake() {
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {

    }

    private void OnEnable() {
        soundManager.OnMute += OnMuteGame;
        gameManager.OnEndGame += EndGame;
    }

    private void OnMuteGame(bool mute) {
        audioSource.mute = mute;
    }

    private void EndGame(bool isWin) {
        if(isWin) {
            audioSource.clip = audioWin;
        } else {
            audioSource.clip = audioLose;
        }
        Invoke("PlayAudioEndGame", 0.3f);
    }

    private void OnDisable() {
        soundManager.OnMute -= OnMuteGame;
        gameManager.OnEndGame -= EndGame;
    }

    private void PlayAudioEndGame() {
        audioSource.Play();
    }
}
