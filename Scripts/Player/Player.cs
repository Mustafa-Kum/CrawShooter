using UnityEngine;

public class Player : MonoBehaviour, ITeamInterface
{
    private CameraController cameraController;
    private Camera mainCamera;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 aimInput;
    private float animatorTurnSpeed;
    private bool canAttack = true;
    public bool isDead = false;
    
    [Header("Components")]
    [Space]
    [SerializeField] private Joystick moveStick;
    [SerializeField] private Joystick aimStick;
    [SerializeField] private MovementComponent movementComponent;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private UIManager uiManager;

    [Header("Team")]
    [Space]
    [SerializeField] private int teamID = 1;

    [Header("Health And Stamina")]
    [Space]
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private AbilityComponent abilityComponent;
    [SerializeField] private PlayerHealthBar healthBar;
    [SerializeField] private PlayerStaminaBar staminaBar;

    [Header("Inventory")]
    [Space]
    public InventoryComponent inventoryComponent;

    [Header("Movement")]
    [Space]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float minMoveSpeed = 20f;
    [SerializeField] private float maxMoveSpeed = 30f;
    [SerializeField] private float animTurnSpeed = 30f;


    public int GetTeamID()
    {
        return teamID;
    }

    private void Start()
    {
        InitializeComponents();
        InitializeUI();
        InitializeGame();
    }

    private void InitializeComponents()
    {
        moveStick.onStickInputValueUpdated += moveInputUpdated;
        aimStick.onStickInputValueUpdated += aimInputUpdated;
        aimStick.onStickTabbed += StartSwitchWeapon;

        mainCamera = Camera.main;
        cameraController = FindAnyObjectByType<CameraController>();
        animator = GetComponent<Animator>();
    }

    private void InitializeUI()
    {
        healthComponent.onHealthChange += HealthChanged;
        healthComponent.onDead += StartDeathSequence;
        abilityComponent.onStaminaChanged += StaminaChanged;
        abilityComponent.BroadcastStaminaChanged();
        healthComponent.HealthValueUpdate();
    }

    private void InitializeGame()
    {
        GamePlay.GameStarted();
    }

    private void StaminaChanged(float newValue, float maxValue)
    {
        staminaBar.UpdateStamina(newValue, 0, maxValue);
    }

    private void StartDeathSequence(GameObject killer)
    {
        if (!isDead)
        {
            isDead = true;
            canAttack = false;

            animator.SetBool("attacking", false);

            animator.SetTrigger("Death");
            animator.SetLayerWeight(2, 1);

            uiManager.SetGamePlayControlEnabled(false);
        }
    }

    private void HealthChanged(float health, float delta, float maxHealth)
    {
        healthBar.UpdateHealth(health, delta, maxHealth);
    }

    public void AttackPoint()
    {
        inventoryComponent.GetActiveWeapon().Attack();
    }

    private void StartSwitchWeapon()
    {
        animator.SetTrigger("switchWeapon");
    }

    public void SwitchWeapon()
    {
        inventoryComponent.NextWeapon();
    }

    private void aimInputUpdated(Vector2 inputValue)
    {
        if (canAttack)
        {
            aimInput = inputValue;

            if (aimInput.magnitude > 0)
            {
                animator.SetBool("attacking", true);
            }
            else
            {
                animator.SetBool("attacking", false);
            }
        }
    }

    private void moveInputUpdated(Vector2 inputValue)
    {
        moveInput = inputValue;
    }

    void Update()
    {
        if (!isDead)
        {
            MovementAndAim();
            UpdateCamera();
        }
    }

    Vector3 StickInputToDirection(Vector3 inputValue)
    {
        Vector3 rightDir = mainCamera.transform.right;
        Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * inputValue.x + upDir * inputValue.y;
    }

    private void MovementAndAim()
    {
        if (!isDead)
        {
            Vector3 moveDir = StickInputToDirection(moveInput);

            characterController.Move(moveDir * Time.deltaTime * moveSpeed);

            UpdateAim(moveDir);

            float forward = Vector3.Dot(moveDir, transform.forward);
            float right = Vector3.Dot(moveDir, transform.right);

            animator.SetFloat("forwardSpeed", forward);
            animator.SetFloat("rightSpeed", right);

            characterController.Move(Vector3.down * Time.deltaTime * 10f);

        }
    }

    private void UpdateAim(Vector3 currentMoveDirection)
    {
        Vector3 aimDir = currentMoveDirection;

        if (aimInput.magnitude != 0) // Aiming
        {
            aimDir = StickInputToDirection(aimInput);
        }

        RotatingAim(aimDir);
    }

    private void UpdateCamera()
    {
        if (moveInput.magnitude != 0 && aimInput.magnitude == 0 && cameraController != null)
            cameraController.AddRotateInput(moveInput.x);
    }

    private void RotatingAim(Vector3 aimDir)
    {
        float currentTurnSpeed = movementComponent.RotatingAim(aimDir);

        animatorTurnSpeed = Mathf.Lerp(animatorTurnSpeed, currentTurnSpeed, Time.deltaTime * animTurnSpeed);

        animator.SetFloat("turnSpeed", animatorTurnSpeed);
    }

    internal void AddMoveSpeed(float boostValue)
    {
        if (!isDead)
        {
            moveSpeed += boostValue;
            moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
        }
    }

    public void DeathFinished()
    {
        uiManager.SwitchToDeathMenu();
    }
}
