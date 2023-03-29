using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standupTime;
    [SerializeField] private float maxHealth;
    private float health, knockTimer;
    private int hitHash, knockHash, standupHash, deathHash, dyingHash, revivalHash;
    [SerializeField] private bool knocking;
    private bool destroyed;
    private Animator animator;
    private CharacterController controller;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public event Action OnTakeDamageEnd;
    private UIMenu ui;
    [Header("VFX")]
    [SerializeField] private EffectObjectPool hitEffect;
    [SerializeField] private EffectObjectPool knockDownVFX;
    private AbsSpecialSkill specialSkill;
    private bool skillCasting;
    private ObjectPoolerManager objectPoolerManager;
    private PlayerController playerController;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        standupHash = Animator.StringToHash("standup");
        deathHash = Animator.StringToHash("death");
        dyingHash = Animator.StringToHash("dying");
        revivalHash = Animator.StringToHash("revival");
        healthBarPlayer = FindObjectOfType<HealthBarPlayer>();
        ui = FindObjectOfType<UIMenu>();
        objectPoolerManager = ObjectPoolerManager.Instance;
        specialSkill = GetComponent<AbsSpecialSkill>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (specialSkill != null)
        {
            specialSkill.OnStart += CastSkillStart;
            specialSkill.OnDone += CastSkillEnd;
        }
    }

    private void OnDisable()
    {
        if (specialSkill != null)
        {
            specialSkill.OnStart -= CastSkillStart;
            specialSkill.OnDone -= CastSkillEnd;
        }
    }

    private void Update()
    {
        if (knocking)
        {
            knockTimer += Time.deltaTime;
            float speed = knockBackCuvre.Evaluate(knockTimer) * 10f;
            controller.SimpleMove(-transform.forward.normalized * speed);
        }
    }

    private void Start()
    {
        health = maxHealth;
        healthBarPlayer?.CreateHealthBar(maxHealth);
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;

            objectPoolerManager.SpawnObject(hitEffect, hitPoint, Quaternion.identity);
            ui?.DisplayHitPoint(false);
            healthBarPlayer?.UpdateHealthBarValue(health);

            if (health <= 0)
            {
                Destroy(attackType);
                return;
            }

            if (!knocking && !skillCasting)
            {
                if (attackType == AttackType.light)
                {
                    animator.SetTrigger(hitHash);
                }
                else
                {
                    Vector3 dirAttack = hitPoint - transform.position;
                    transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
                    knocking = true;
                    animator.SetTrigger(knockHash);
                }
            }
            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);

            if (skillCasting)
            {
                TakeDamagaEnd();
            }

        }
    }

    public void KnockDone()
    {
        knocking = false;
        knockTimer = 0;
        if (!destroyed)
        {
            Invoke("StandUp", standupTime);
        }
    }

    private void StandUp()
    {
        animator.SetTrigger(standupHash);
    }

    public void TakeDamagaEnd()
    {
        OnTakeDamageEnd?.Invoke();
    }

    private void Destroy(AttackType attackType)
    {
        destroyed = true;
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            knocking = true;
            animator.SetTrigger(knockHash);
            animator.SetBool(dyingHash, true);
        }
        gameManager.GameLose();
    }

    public void Revival() {
        gameObject.SetActive(false);
        animator.SetTrigger(revivalHash);
        playerController.isStart = true;
        GameObject newPlayer = Instantiate(gameObject, transform.position, transform.rotation);
        gameManager.player = newPlayer.transform;
        newPlayer.SetActive(true);
        destroyed = false;
        gameManager.virtualCamera.Follow = newPlayer.transform;
    }

    //Attach Animation Event
    public void KnockDownEffect()
    {
        CinemachineShake.Instance.ShakeCamera(5, .2f);
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
    }

    private void CastSkillStart()
    {
        skillCasting = true;
    }

    private void CastSkillEnd()
    {
        skillCasting = false;
    }
}
