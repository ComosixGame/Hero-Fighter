using System;
using UnityEngine;
using UnityEngine.InputSystem;
using MyCustomAttribute;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerInputSystem playerInputSystem;
    private Animator animator;
    private CharacterController characterController;
    private Vector3 direction;
    [SerializeField] private float speed = 20f;
    private int velocityHash;
    private int attackHash;
    private int stateTimeHash;
    private bool isMoveState;
    private bool isReady = true;
    private float stateTimeAnim;
    [SerializeField, ReadOnly] private bool attacking;
    private AbsSkill[] skills;
    [SerializeField, ReadOnly] private AbsSkill skill1;
    [SerializeField, ReadOnly] private AbsSkill skill2;
    [SerializeField, ReadOnly] private AbsSkill skill3;
    [SerializeField, ReadOnly] private AbsSkill skill4;
    [SerializeField] private PlayerAttack[] playerAttacks;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInputSystem = new PlayerInputSystem();

        foreach (PlayerAttack playerAttack in playerAttacks)
        {
            playerAttack.gameObject.SetActive(false);
        }

        velocityHash = Animator.StringToHash("Velocity");
        attackHash = Animator.StringToHash("Attack");
        stateTimeHash = Animator.StringToHash("StateTime");
        AddSkill();

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

        //lắng nghe skill thực hiện xong
        foreach (AbsSkill skill in skills)
        {
            skill.OnDoneExecuting += DoneExecutingSkill;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady && isMoveState)
        {
            Move();
            RotationLook();
        }

        HandleAnimation();
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        stateTimeAnim = animationState.normalizedTime;
        isMoveState = animationState.IsName("Move");
        animator.SetFloat(stateTimeHash, Mathf.Repeat(stateTimeAnim, 1f));
    }

    private void GetDirectionMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        direction = new Vector3(dir.x, 0, dir.y);
    }

    private void RotationLook()
    {
        // if (direction.x < 0)
        // {
        //     Quaternion rot = Quaternion.LookRotation(Vector3.left);
        //     transform.rotation = rot;
        // }
        // else if (direction.x > 0)
        // {
        //     Quaternion rot = Quaternion.LookRotation(Vector3.right);
        //     transform.rotation = rot;
        // }

        if (direction != Vector3.zero)
        {
            Quaternion rotLook = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotLook, 20f * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector3 motionMove = direction * speed;
        characterController.SimpleMove(motionMove);
    }

    private void HandleAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float Velocity = horizontalVelocity.magnitude / speed;
        if (Velocity > 0)
        {
            animator.SetFloat(velocityHash, Velocity);
        }
        else
        {
            float v = animator.GetFloat(velocityHash);
            v = v > 0.10f ? Mathf.Lerp(v, 0, 20f * Time.deltaTime) : 0;
            animator.SetFloat(velocityHash, v);
        }
    }

    private void ActiveSkill(InputAction.CallbackContext ctx)
    {
        if (!isReady) return;
        switch (ctx.control.displayName)
        {
            case "1":
                isReady = false;
                skill1.Cast();
                break;
            case "2":
                isReady = false;
                skill2.Cast();
                break;
            case "3":
                isReady = false;
                skill3.Cast();
                break;
            case "4":
                isReady = false;
                skill4.Cast();
                break;
            default:
                throw new InvalidOperationException("key invalid");
        }
    }

    private void HandleAttack(InputAction.CallbackContext ctx)
    {
        animator.ResetTrigger(attackHash);
        animator.SetTrigger(attackHash);
    }

    private void DoneExecutingSkill()
    {
        isReady = true;
    }

    private void AddSkill()
    {
        skills = GetComponents<AbsSkill>();

        if (skills.Length > 4)
        {
            throw new InvalidOperationException("skills Length more than 4");
        }

        foreach (AbsSkill skill in skills)
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
        playerAttacks[index].gameObject.SetActive(true);
    }

    public void AttackEnd(int index)
    {
        playerAttacks[index].gameObject.SetActive(false);

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

        foreach (AbsSkill skill in skills)
        {
            skill.OnDoneExecuting -= DoneExecutingSkill;
        }

        playerInputSystem.Disable();
    }
}
