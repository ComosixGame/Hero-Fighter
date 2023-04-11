using UnityEngine;

public abstract class AbsMagic : MonoBehaviour
{
    protected ParticleSystem effect;
    [SerializeField] protected LayerMask targetLayer;
    
    private void Awake() {
        effect = GetComponent<ParticleSystem>();
    }

    public abstract void Cast();
}
