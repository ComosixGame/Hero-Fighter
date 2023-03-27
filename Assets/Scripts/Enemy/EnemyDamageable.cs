using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standUpTime;
    private float health;
    public bool destroyed;
    private int stepBackHash;
    private int hitHash;
    private int knockHash;
    private int deathHash;
    private int dyingHash;
    private int standUpHash;
    public bool knockBack { get; private set; }
    private float knockTimer;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private Collider colliderGameObject;
    private GameManager gameManager;
    private AbsEnemyAttack absEnemyAttack;
    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public GameObject DestroyedBody;
    public event Action OnTakeDamageEnd;
    [SerializeField] private HealthBarRennder healthBarRennder = new HealthBarRennder();
    private UIMenu uI;

    [Header("VFX")]
    [SerializeField] private EffectObjectPool hitEffect;
    [SerializeField] private EffectObjectPool knockDownVFX;

    private ObjectPoolerManager objectPoolerManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        colliderGameObject = GetComponent<Collider>();
        stepBackHash = Animator.StringToHash("StepBack");
        hitHash = Animator.StringToHash("Hit");
        knockHash = Animator.StringToHash("Knock");
        deathHash = Animator.StringToHash("death");
        dyingHash = Animator.StringToHash("dying");
        standUpHash = Animator.StringToHash("StandUp");
        absEnemyAttack = GetComponent<AbsEnemyAttack>();
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void OnEnable() {
        uI = FindObjectOfType<UIMenu>();
        gameManager.OnStartGame += StartGame;
        gameManager.OnEndGame += EndGame;
        enemyBehaviour.enabled = true;
        colliderGameObject.enabled = true;
    }

    private void OnDisable() {
        gameManager.OnNewGame -= NewGame;
        gameManager.OnEndGame -= EndGame;
    }

    private void StartGame()
    {
        destroyed = false;
        SetInitHealthBar(maxHealth);
        health = maxHealth;
        gameManager.OnStartGame -= StartGame;
    }

    private void LateUpdate()
    {
        gameManager.OnNewGame += NewGame;
        if (healthBarRennder._healthBar == null)
        {
            SetInitHealthBar(maxHealth);
            healthBarRennder.UpdateHealthBarRotation();
            health = maxHealth;
        }else
        {
            healthBarRennder.UpdateHealthBarRotation();
        }
    }

    private void Update()
    {
        if (knockBack)
        {
            knockTimer += Time.deltaTime;
            float speed = knockBackCuvre.Evaluate(knockTimer) * 10f;
            agent.Move(-transform.forward.normalized * speed * Time.deltaTime);
        }
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            uI?.DisplayHitPoint(true);
            AttackVFX(hitPoint);
            health -= damage;
            healthBarRennder.UpdateHealthBarValue(health);
            if (health <= 0)
            {
                destroyed = true;
                Destroy(attackType);
                return;
            }
            HandleHitReaction(hitPoint, attackType);
            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);
        }
    }

    private void HandleHitReaction(Vector3 hitPoint, AttackType attackType)
    {
        Vector3 dir = hitPoint - transform.position;
        transform.rotation = Ultils.GetRotationLook(dir, transform.forward);
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(hitHash);
        }
        else
        {
            knockBack = true;
            animator.SetTrigger(knockHash);
            animator.ResetTrigger(standUpHash);
        }
    }

    public void TakeDamagaEnd()
    {
        OnTakeDamageEnd?.Invoke();
    }

    public void KnockEnd()
    {
        knockBack = false;
        knockTimer = 0;
        if (!destroyed)
        {
            Invoke("StandUp", standUpTime);
        }
    }

    private void StandUp()
    {
        animator.SetTrigger(standUpHash);
    }

    public void SetInitHealthBar(float health)
    {
        healthBarRennder.CreateHealthBar(transform, maxHealth);
    }

    //Attach Animation Event
    public void KnockDownVFX()
    {
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(5, .1f);
    }

    private void AttackVFX(Vector3 pos)
    {
        objectPoolerManager.SpawnObject(hitEffect, pos, Quaternion.identity);
    }

    private void Destroy(AttackType attackType)
    {
        // GameObject deadBody = Instantiate(DestroyedBody, transform.position, transform.rotation);
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            knockBack = true;
            animator.SetTrigger(knockHash);
            animator.SetBool(dyingHash, true);
        }

        enemyBehaviour.enabled = false;
        colliderGameObject.enabled = false;
        healthBarRennder.DestroyHeathBar();
        agent.ResetPath();
        destroyed = true;
    }

    private void NewGame()
    {
        healthBarRennder.DestroyHeathBar();
    }

    private void EndGame(bool win)
    {

    }

}
