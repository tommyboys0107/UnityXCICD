using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanAnimator : MonoBehaviour
{
    Animator animator = null;

    public void UpdateVelocity(float velocity)
    {
        animator.SetFloat("Velocity", velocity);
    }

    public void PerformLightAttack()
    {
        if (!IsInAnimationState("LightAttack") && !IsNextAnimationState("LightAttack"))
        {
            animator.SetTrigger("IsLightAttack");
        }
    }

    public void PerformHeavyAttack()
    {
        if (!IsInAnimationState("HeavyAttack") && !IsNextAnimationState("HeavyAttack"))
        {
            animator.SetTrigger("IsHeavyAttack");
        }
    }

    public bool IsInAnimationState(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public bool IsNextAnimationState(string stateName)
    {
        return animator.GetNextAnimatorStateInfo(0).IsName(stateName);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
}
