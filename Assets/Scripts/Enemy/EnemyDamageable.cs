using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamageable : MonoBehaviour
{
    public AudioClip audioClip, deathAudioClip;
    [Range(0,1)] public float volumeScale;
    public UnityEvent<Vector3> OnTakeDamge;
    private SoundManager soundManager;
    private float standUpTimer = 2f;

    private void Awake() 
    {
        soundManager = SoundManager.Instance;
    }

    public void TakeDamge(Vector3 hitPoint, Vector3 force, float damage)
    {

    }

}
