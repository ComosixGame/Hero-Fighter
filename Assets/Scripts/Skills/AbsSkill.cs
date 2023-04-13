using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;

// public enum skillHolder
// {
//     skill1,
//     skill2,
//     skill3,
//     skill4
// }

public abstract class AbsSkill : MonoBehaviour
{
    [SerializeField, ReadOnly] private float cooldownTimer;
    public string skillName;
    public string skillDescription;
    // public skillHolder skillHolder;
    [SerializeField] private float cooldownTime;
    private bool ready = true;
    public event Action<float> OnCoolDown;
    public event Action OnDone;

    protected abstract void Action();
    public bool Cast()
    {
        if (ready)
        {
            ready = false;
            Action();
            StartCoroutine(CoolDownCoroutine());
            return true;
        }

        return false;
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

    public void Done()
    {
        OnDone?.Invoke();
    }
}
