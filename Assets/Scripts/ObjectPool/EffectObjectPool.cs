using UnityEngine;

public class EffectObjectPool : GameObjectPool
{
    private ParticleSystem particle;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(!particle.IsAlive(false)) {
            ObjectPoolerManager.DeactiveObject(this);
        }
    }
}
