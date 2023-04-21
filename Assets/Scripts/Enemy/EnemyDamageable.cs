using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float health;
    public bool destroyed;
    private int hitHash;
    private int hitContinuousHash;
    private int knockHash;
    private int deathHash;
    private int deathKnockHash;
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private Collider colliderGameObject;
    private Coroutine coroutine;
    private GameManager gameManager;
    [SerializeField] private HealthBarRennder healthBarRennder = new HealthBarRennder();
    private UIMenu uI;
    private LoadSceneManager loadSceneManager;

    [Header("VFX")]
    [SerializeField] private EffectObjectPool knockDownVFX;
    private ObjectPoolerManager objectPoolerManager;
    public UnityEvent OnDestroy;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        animator = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        colliderGameObject = GetComponent<Collider>();

        hitHash = Animator.StringToHash("Hit");
        hitContinuousHash = Animator.StringToHash("HitContinuous");
        knockHash = Animator.StringToHash("Knock");
        deathHash = Animator.StringToHash("death");
        deathKnockHash = Animator.StringToHash("deathKnock");
        objectPoolerManager = ObjectPoolerManager.Instance;
        healthBarRennder.CreateHealthBar(transform, maxHealth);
    }

    private void OnEnable()
    {
        uI = FindObjectOfType<UIMenu>();
        enemyBehaviour.enabled = true;
        colliderGameObject.enabled = true;
        destroyed = false;
        health = maxHealth;
        healthBarRennder.UpdateHealthBarValue(health);
        healthBarRennder.SetActive(true);
        loadSceneManager.OnStart += CancelDissolve;
    }

    private void OnDisable()
    {
        CancelInvoke("Hide");
        loadSceneManager.OnStart -= CancelDissolve;
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if (!animationState.IsName("KnockDown"))
        {
            animator.ResetTrigger(knockHash);
        }

        if (!animationState.IsName("Hit"))
        {
            animator.ResetTrigger(hitHash);
        }

        if (!animationState.IsName("HitContinuous"))
        {
            animator.ResetTrigger(hitContinuousHash);
        }
    }

    private void LateUpdate()
    {
        healthBarRennder.UpdateHealthBarRotation();
    }

    public void TakeDamgae(Vector3 dirAttack, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            animator.applyRootMotion = true;
            uI?.DisplayHitPoint(true);
            health -= damage;
            healthBarRennder.UpdateHealthBarValue(health);
            if (health <= 0)
            {
                destroyed = true;
                Destroy(attackType);
                return;
            }
            HandleHitReaction(dirAttack, attackType);
        }
    }

    private void HandleHitReaction(Vector3 dirAttack, AttackType attackType)
    {
        transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
        switch (attackType)
        {
            case AttackType.heavy:
                animator.SetTrigger(knockHash);
                break;
            case AttackType.continuous:
                animator.SetTrigger(hitContinuousHash);
                break;
            default:
                animator.SetTrigger(hitHash);
                break;
        }
    }

    private void Destroy(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.heavy:
                animator.SetTrigger(deathKnockHash);
                break;
            default:
                animator.SetTrigger(deathHash);
                break;
        }

        enemyBehaviour.enabled = false;
        colliderGameObject.enabled = false;
        healthBarRennder.SetActive(false);
        destroyed = true;
        Invoke("Hide", 3f);
        OnDestroy?.Invoke();
    }

    private void Hide()
    {
        coroutine = StartCoroutine(Dissolve());
    }

    private void CancelDissolve()
    {
        StopCoroutine(coroutine);
        SkinnedMeshRenderer skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        skinned.enabled = true;
    }

    private IEnumerator Dissolve()
    {
        SkinnedMeshRenderer skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < 4; i++)
        {
            skinned.enabled = !skinned.enabled;
            yield return new WaitForSeconds(0.3f);
        }

        objectPoolerManager.DeactiveObject(GetComponent<GameObjectPool>());
    }

    public void KnockDownEffect()
    {
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(5, .1f);
    }
}
