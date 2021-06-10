using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanController : MonoBehaviour
{
    /// <summary>
    /// Define the name for input horizontal axis.
    /// </summary>
    public string horizontalAxisName = "Horizontal";
    /// <summary>
    /// Define the name for input vertical axis.
    /// </summary>
    public string verticalAxisName = "Vertical";

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

    [Header("FOV transition")]
    /// <summary>
    /// Define normal FOV of camera.
    /// </summary>
    public float normalFOV = 60.0f;
    /// <summary>
    /// Define FOV of camera when the player is sprinting.
    /// </summary>
    public float sprintFOV = 65.0f;
    /// <summary>
    /// Define the transition time between FOVs.
    /// </summary>
    public float FOVTransitionTime = 0.5f;

    FootmanAnimator footmanAnimator = null;
    Rigidbody rigid = null;
    float inputHorizontal = 0.0f;
    float inputVertical = 0.0f;
    Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        footmanAnimator = gameObject.GetComponent<FootmanAnimator>();
        rigid = gameObject.GetComponent<Rigidbody>();
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
        inputHorizontal = Input.GetAxis(horizontalAxisName);
        inputVertical = Input.GetAxis(verticalAxisName);

        if (CanMove())
        {
            Vector3 rawMoveDirection = new Vector3(inputHorizontal, 0.0f, inputVertical);

            moveDirection = Vector3.ClampMagnitude(rawMoveDirection, 1.0f);
            footmanAnimator.UpdateVelocity(moveDirection.sqrMagnitude);
        }
        else
        {
            moveDirection = Vector3.zero;
            footmanAnimator.UpdateVelocity(0.0f);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            footmanAnimator.PerformLightAttack();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            footmanAnimator.PerformHeavyAttack();
        }
    }

    private bool CanMove()
    {
        return (!Mathf.Approximately(inputHorizontal, 0.0f) || !Mathf.Approximately(inputVertical, 0.0f))
            && !footmanAnimator.IsInAnimationState("LightAttack") && !footmanAnimator.IsInAnimationState("HeavyAttack");
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
