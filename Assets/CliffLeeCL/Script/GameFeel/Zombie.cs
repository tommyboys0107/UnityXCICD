using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Collider zombieCol = null;
    public Collider sight = null;
    public int healthPoint = 5;
    public int attackDamage = 1;
    public float movingSpeed = 1.0f;
    public float angularSpeed = 10.0f;
    [Header("Ragdoll")]
    public bool canUseRagdoll = false;
    public GameObject ragdollObject = null;
    public Rigidbody ragdollRigidbody = null;
    public float ragdollForceMultiplier = 7.0f;

    Animator animator = null;
    Rigidbody rigid = null;
    Transform playerTransform = null;
    bool isDead = false;

    public void TakeDamage(int amount)
    {
        healthPoint -= amount;

        if (!isDead && healthPoint <= 0)
        {
            zombieCol.enabled = false;
            sight.enabled = false;
            isDead = true;
            animator.SetTrigger("IsDead");

            if (canUseRagdoll)
            {
                gameObject.SetActive(false);
                ragdollObject.transform.parent = null;
                ragdollObject.SetActive(true);
            }
            SlowMotion.Instance.PlaySlowMotion(0.2f, 0.5f, 1.0f, 0.5f);
        }
        else
        {
            animator.SetTrigger("IsHurt");
        }
    }

    public void TakeDamage(int amount, Vector3 knockbackDirection, float forceMultiplier)
    {
        TakeDamage(amount);
        Knockback.Instance.KnockbackTarget(rigid, knockbackDirection, forceMultiplier);
        if (isDead && canUseRagdoll)
        {
            Knockback.Instance.KnockbackTarget(ragdollRigidbody, knockbackDirection, forceMultiplier * ragdollForceMultiplier);
        }
    }

    public void ToggleRagdollEffect(bool value)
    {
        canUseRagdoll = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    void OnTriggerStay(Collider otherCol)
    {
        if (!playerTransform && otherCol.CompareTag("Player"))
        {
            playerTransform = otherCol.transform;
            animator.SetBool("IsPursue", true);
        }
    }

    private void UpdateMovement()
    {
        if (CanMove())
        {
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
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

    private bool CanMove()
    {
        return !isDead && !IsInAnimationState("Hurt") && playerTransform;
    }

    private bool IsInAnimationState(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
