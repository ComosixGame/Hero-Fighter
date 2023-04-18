using System;
using UnityEngine;
using UnityEngine.InputSystem;
using MyCustomAttribute;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private enum State
    {
        enable,
        disable,

    }
    private State state;
    public PlayerInputSystem playerInputSystem;
    private Animator animator;
    private CharacterController characterController;
    private Vector3 direction;
    [SerializeField] private float speed = 20f;
    private int velocityXHash;
    private int velocityZHash;
    private int attackHash;
    private int stateTimeHash;
    private float stateTimeAnim;
    private bool disable;
    private Vector3 motionMove;
    private Coroutine attackWaitCoroutine;
    private AbsPlayerSkill[] skills;
    [SerializeField] private AbsPlayerSkill skill1;
    [SerializeField] private AbsPlayerSkill skill2;
    [SerializeField] private AbsPlayerSkill skill3;
    [SerializeField] private AbsPlayerSkill skill4;
    [SerializeField, ReadOnly] private AbsSpecialSkill specialskill;
    [SerializeField] private PlayerHurtBox[] playerHurtBoxes;
    private GameManager gameManager;
    private SoundManager soundManager;
    public bool isStart;
    public AbsPlayerSkill[] absPlayerSkills;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        characterController = GetComponent<CharacterController>();
        playerInputSystem = new PlayerInputSystem();

        foreach (PlayerHurtBox playerHurtBox in playerHurtBoxes)
        {
            playerHurtBox.gameObject.SetActive(false);
        }

        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
        attackHash = Animator.StringToHash("Attack");
        stateTimeHash = Animator.StringToHash("StateTime");
        

    }

    private void OnEnable()
    {
        playerInputSystem.Enable();
        playerInputSystem.Player.Move.performed += GetDirectionMove;
        playerInputSystem.Player.Move.canceled += GetDirectionMove;
        playerInputSystem.Player.Attack.started += HandleAttack;
        playerInputSystem.Player.Skil1.started += ActiveSkill;
        playerInputSystem.Player.Skil2.started += ActiveSkill;
        playerInputSystem.Player.Skil3.started += ActiveSkill;
        playerInputSystem.Player.Skil4.started += ActiveSkill;
        playerInputSystem.Player.SpecialSkil.started += ActiveSpecialSkill;

        gameManager.OnInitUiDone += StartGame;

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isStart)
        {
            switch (state)
            {
                case State.disable:
                    characterController.enabled = false;
                    break;
                default:
                    characterController.enabled = true;
                    Move();
                    RotationLook();
                    break;
            }

            HandleAnimation();
        }
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        stateTimeAnim = animationState.normalizedTime;
        animator.SetFloat(stateTimeHash, Mathf.Repeat(stateTimeAnim, 1f));
        if (animationState.IsName("Move"))
        {
            state = State.enable;
        }
        else
        {
            state = State.disable;
        }
    }

    private void GetDirectionMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        direction = new Vector3(dir.x, 0, dir.y);
    }

    private void RotationLook()
    {
        transform.rotation = Ultils.GetRotationLook(direction, transform.forward);
    }

    private void Move()
    {
        float s = speed;
        if (direction.z != 0 && direction.x == 0)
        {
            s = speed / 1.5f;
        }
        motionMove = direction * s;
        characterController.SimpleMove(motionMove);
        //soundManager.PlaySound(walkSound);
    }

    private void HandleAnimation()
    {
        Vector3 Velocity = new Vector3(motionMove.x, 0, motionMove.z).normalized;
        if (Velocity.magnitude > 0)
        {
            animator.SetFloat(velocityXHash, Math.Abs(Velocity.x));
            bool isLookRight = transform.forward.normalized == Vector3.right;
            animator.SetFloat(velocityZHash, isLookRight ? -Velocity.z : Velocity.z);
        }
        else
        {
            float vX = animator.GetFloat(velocityXHash);
            float vZ = animator.GetFloat(velocityZHash);
            vX = vX > 0.10f ? Mathf.Lerp(vX, 0, 20f * Time.deltaTime) : 0;
            vZ = vZ > 0.10f || vZ < -0.1f ? Mathf.Lerp(vZ, 0, 20f * Time.deltaTime) : 0;
            animator.SetFloat(velocityXHash, vX);
            animator.SetFloat(velocityZHash, vZ);
        }
    }

    private void ActiveSkill(InputAction.CallbackContext ctx)
    {
        switch (ctx.control.displayName)
        {
            case "1":
                if ((SkillSystem.currentEnergy >= skill1.energy) && (skill1.cooldownTimer <= 0))
                {
                    skill1?.Cast(skillHolder.skill1);
                }
                else if (SkillSystem.currentEnergy <= skill1.energy)
                {
                    gameManager.NotEnoughEnergy();
                }
                break;
            case "2":
                if ((SkillSystem.currentEnergy >= skill2.energy) && (skill2.cooldownTimer <= 0))
                {
                    skill2?.Cast(skillHolder.skill2);
                }
                else if (SkillSystem.currentEnergy <= skill2.energy)
                {
                    gameManager.NotEnoughEnergy();
                }
                break;
            case "3":
                if ((SkillSystem.currentEnergy >= skill3.energy) && (skill3.cooldownTimer <= 0))
                {
                    skill3?.Cast(skillHolder.skill3);
                }
                else if (SkillSystem.currentEnergy <= skill3.energy)
                {
                    gameManager.NotEnoughEnergy();
                }
                break;
            case "4":
                if ((SkillSystem.currentEnergy >= skill4.energy) && (skill4.cooldownTimer <= 0))
                {
                    skill4?.Cast(skillHolder.skill4);
                }
                else if (SkillSystem.currentEnergy <= skill4.energy)
                {
                    gameManager.NotEnoughEnergy();
                }
                break;
            default:
                throw new InvalidOperationException("key invalid");
        }
    }

    private void ActiveSpecialSkill(InputAction.CallbackContext ctx)
    {
        specialskill?.Cast();
    }

    private void HandleAttack(InputAction.CallbackContext ctx)
    {
        if (attackWaitCoroutine != null)
        {
            StopCoroutine(attackWaitCoroutine);
        }
        attackWaitCoroutine = StartCoroutine(AttackWait());
    }

    IEnumerator AttackWait()
    {
        animator.SetTrigger(attackHash);
        yield return new WaitForSeconds(0.3f);
        animator.ResetTrigger(attackHash);
    }

    public void AddSkill(AbsPlayerSkill[] skillsAvailable, AbsSpecialSkill specialSkillAvailable)
    {
        specialskill = specialSkillAvailable;
        skills = skillsAvailable;

        if (skills.Length > 4)
        {
            throw new InvalidOperationException("skills Length more than 4");
        }

        foreach (AbsPlayerSkill skill in skills)
        {
            switch (skill.skillHolder)
            {
                case skillHolder.skill1:
                    skill1 = skill;
                    break;
                case skillHolder.skill2:
                    skill2 = skill;
                    break;
                case skillHolder.skill3:
                    skill3 = skill;
                    break;
                case skillHolder.skill4:
                    skill4 = skill;
                    break;
                default:
                    throw new InvalidOperationException("invalid skill skillHolder");
            }
        }
    }

    public void AttackStart(int index)
    {
        if (!disable)
        {
            playerHurtBoxes[index].gameObject.SetActive(true);
        }
    }

    public void AttackEnd(int index)
    {
        playerHurtBoxes[index].gameObject.SetActive(false);

    }

    private void StartGame()
    {
        isStart = true;
    }

    private void OnDisable()
    {
        playerInputSystem.Player.Move.performed -= GetDirectionMove;
        playerInputSystem.Player.Move.canceled -= GetDirectionMove;
        playerInputSystem.Player.Attack.started -= HandleAttack;
        playerInputSystem.Player.Skil1.started -= ActiveSkill;
        playerInputSystem.Player.Skil2.started -= ActiveSkill;
        playerInputSystem.Player.Skil3.started -= ActiveSkill;
        playerInputSystem.Player.Skil4.started -= ActiveSkill;
        playerInputSystem.Player.SpecialSkil.started -= ActiveSpecialSkill;

        playerInputSystem.Disable();

        gameManager.OnInitUiDone -= StartGame;
    }
}
