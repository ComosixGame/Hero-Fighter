using UnityEngine;

public class AreaColliderTrigger : MonoBehaviour
{   
    [SerializeField] private UIMenu uIMenu;
    [SerializeField] private MapGeneration mapGeneration;
    [SerializeField] private LayerMask playerLayer;

    private SoundManager soundManager;
    public AudioClip backgroundSound;

    private void Awake() {
        soundManager = SoundManager.Instance;
    }

    private void Start() {
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & 1 << other.gameObject.layer) != 0)
        {
            uIMenu.PreviousAnimation(false);
            if(mapGeneration != null)
            {
                mapGeneration.NextWave();
                gameObject.SetActive(false);
                soundManager.SetMusicbackGround(backgroundSound);
                soundManager.SetPlayMusic(true);
            }
            
        }
    }
}
