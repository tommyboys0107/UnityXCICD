using UnityEngine;
using UnityEngine.InputSystem;

public class FootmanController : MonoBehaviour
{
    DefaultInputActions inputActions;
    
    [Header("Player status")]
    public int healthPoint = 10;
    public int lightAttackDamage = 2;
    public int heavyAttackDamage = 5;
    /// <summary>
    /// Define how fast the player moves.
    /// </summary>
    public float movingSpeed = 1.0f;
    /// <summary>
    /// Define how fast the player rotates.
    /// </summary>
    public float angularSpeed = 20.0f;

    FootmanAnimator footmanAnimator = null;
    Rigidbody rigid = null;
    Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        inputActions = new DefaultInputActions();
        inputActions.Enable();
        footmanAnimator = gameObject.GetComponent<FootmanAnimator>();
        rigid = gameObject.GetComponent<Rigidbody>();
        inputActions.Player.PrimaryAttack.performed += OnPrimaryAttacked;
        inputActions.Player.SecondaryAttack.performed += OnSecondaryAttacked;
    }

    private void OnSecondaryAttacked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            footmanAnimator.PerformHeavyAttack();
        }
    }

    private void OnPrimaryAttacked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            footmanAnimator.PerformLightAttack();
        }
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    private void HandleInput()
    {
        if (CanMove())
        {
            var inputMove = inputActions.Player.Move.ReadValue<Vector2>();
            var rawMoveDirection = new Vector3(inputMove.x, 0.0f, inputMove.y);

            moveDirection = Vector3.ClampMagnitude(rawMoveDirection, 1.0f);
            footmanAnimator.UpdateVelocity(moveDirection.sqrMagnitude);
        }
        else
        {
            moveDirection = Vector3.zero;
            footmanAnimator.UpdateVelocity(0.0f);
        }
    }

    private bool CanMove()
    {
        return !footmanAnimator.IsInAnimationState("LightAttack") && !footmanAnimator.IsInAnimationState("HeavyAttack");
    }

    private void UpdateMovement()
    {
        if (CanMove())
        {
            Vector3 moveVelocity = moveDirection * movingSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = rigid.position + moveVelocity;

            if (moveVelocity != Vector3.zero)
            {
                Quaternion finalRotation = Quaternion.LookRotation((newPosition - rigid.position).normalized);
                Quaternion newRotation = Quaternion.RotateTowards(rigid.rotation, finalRotation, angularSpeed);

                rigid.MoveRotation(newRotation);
            }
            rigid.MovePosition(newPosition);
        }
    }
}
