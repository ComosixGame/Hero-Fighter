using UnityEngine;

public class EffectObjectPool : GameObjectPool
{
    protected ParticleSystem particle;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        particle = GetComponent<ParticleSystem>();
    }

    protected virtual void Update()
    {
        if(!particle.IsAlive()) {
            ObjectPoolerManager.DeactiveObject(this);
        }
    }
}
