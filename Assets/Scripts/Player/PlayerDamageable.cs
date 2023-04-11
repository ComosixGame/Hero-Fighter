using UnityEngine;
using MyCustomAttribute;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField, ReadOnly] private float health;
    private int hitHash, knockHash, deathHash, revivalHash, deathKnock;
    private bool destroyed;
    private Animator animator;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    private UIMenu ui;
    [Header("VFX")]
    [SerializeField] private EffectObjectPool knockDownVFX;
    private ObjectPoolerManager objectPoolerManager;
    private PlayerController playerController;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        deathHash = Animator.StringToHash("death");
        revivalHash = Animator.StringToHash("revival");
        deathKnock = Animator.StringToHash("deathKnock");
        healthBarPlayer = FindObjectOfType<HealthBarPlayer>();
        ui = FindObjectOfType<UIMenu>();
        objectPoolerManager = ObjectPoolerManager.Instance;
        playerController = GetComponent<PlayerController>();
    }


    private void Start()
    {
        health = maxHealth;
        healthBarPlayer?.CreateHealthBar(maxHealth);
    }

    private void LateUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if(!animationState.IsName("Hit")) {
            animator.ResetTrigger(hitHash);
        }
    }

    public void TakeDamgae(Vector3 dirAttack, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;
            ui?.DisplayHitPoint(false);
            healthBarPlayer?.UpdateHealthBarValue(health);

            if (health <= 0)
            {
                Destroy(attackType);
                return;
            }

            if (attackType == AttackType.light)
            {
                animator.SetTrigger(hitHash);
            }
            else
            {
                transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
                animator.SetTrigger(knockHash);
            }
        }
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
            animator.SetTrigger(deathKnock);
        }
        gameManager.GameLose();
    }

    public void Revival()
    {
        gameObject.SetActive(false);
        animator.SetTrigger(revivalHash);
        playerController.isStart = true;
        GameObject newPlayer = Instantiate(gameObject, transform.position, transform.rotation);
        gameManager.player = newPlayer.transform;
        newPlayer.SetActive(true);
        destroyed = false;
        gameManager.virtualCamera.Follow = newPlayer.transform;
    }

    public void KnockDownEffect()
    {
        CinemachineShake.Instance.ShakeCamera(5, .2f);
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
    }
}
