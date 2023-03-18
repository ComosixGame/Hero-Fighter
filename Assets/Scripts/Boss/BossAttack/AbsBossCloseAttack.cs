using UnityEngine;

public abstract class AbsBossCloseAttack : MonoBehaviour
{
    [SerializeField] private float coolDownTime;
    private bool ready = true;
    public bool attacking {get; protected set;}
    public void Attack() {
        if(ready) {
            ready = false;
            attacking = true;
            Action();
            Invoke("AttackReady", coolDownTime);
        }
    }
    public abstract void Action();
    private void AttackReady() {
        ready = true;
    }
}
