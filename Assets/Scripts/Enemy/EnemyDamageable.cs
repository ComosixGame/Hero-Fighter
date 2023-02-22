using System;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    public AudioClip audioClip, deathAudioClip;
    [Range(0,1)] public float volumeScale;
    public event Action<float> OnTakeDamage;
    private SoundManager soundManager;
    private float standUpTimer = 2f;
    [SerializeField] private float timer;

    private void Awake() 
    {
        soundManager = SoundManager.Instance;
    }

    public void TakeDamgae(Vector3 hitPoint, float damage = 0)
    {
        OnTakeDamage?.Invoke(damage);
    }
    
}
