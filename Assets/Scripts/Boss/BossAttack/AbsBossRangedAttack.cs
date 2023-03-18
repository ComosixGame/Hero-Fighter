using UnityEngine;

public abstract class AbsBossRangedAttack : MonoBehaviour
{
    [SerializeField] private float coolDownTime;
    private bool ready = true;
    public bool attacking {get; protected set;}
    public void Attack() {
        if(ready) {
            ready =  false;
            attacking = true;
            Action();
            Invoke("AttackReady", coolDownTime);
        }

        SideActionAttack();
    }

    protected virtual void SideActionAttack() {

    }

    protected abstract void Action();
    private void AttackReady() {
        ready = true;
    }
}
