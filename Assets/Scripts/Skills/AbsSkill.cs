using System;
using System.Collections;
using UnityEngine;

public enum skillHolder{
    skill1,
    skill2,
    skill3,
    skill4
}

public abstract class AbsSkill : MonoBehaviour
{
    public string skillName;
    public string skillDescription;
    public skillHolder skillHolder;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float executionTime;
    private float cooldownTimer;
    private bool ready = true;
    public event Action<float> OnCoolDown;
    public event Action OnDoneExecuting;

    protected abstract void Action();
    public void Cast()
    {
        if (ready)
        {
            ready = false;
            Action();
            StartCoroutine(CoolDownCoroutine());
            Invoke("DoneExecution", executionTime);
        }
    }

    private IEnumerator CoolDownCoroutine()
    {
        cooldownTimer = cooldownTime;
        while (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
            OnCoolDown?.Invoke(cooldownTimer);
            yield return null;
        }
        cooldownTimer = cooldownTime;
        ready = true;
    }

    private void DoneExecution()
    {
        OnDoneExecuting?.Invoke();
    }
}
