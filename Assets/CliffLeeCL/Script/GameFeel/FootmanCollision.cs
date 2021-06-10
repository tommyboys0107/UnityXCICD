using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanCollision : MonoBehaviour
{
    public Collider kickHitBox = null;
    public Collider slashHitBox = null;

    [Header("Hit Effect")]
    public GameObject kickHitEffect = null;
    public GameObject slashHitEffect = null;
    public Vector3 effectOffset = Vector3.zero;
    public bool enableHitEffect = true;

    [Header("Weapon Trail")]
    public ParticleSystem weaponTrail;
    public bool enableWeaponTrail = true;

    FootmanController footmanController = null;
    Animator animator = null;

    public void ToggleEnableHitEffect(bool value)
    {
        enableHitEffect = value;
    }

    public void ToggleEnableWeaponTrail(bool value)
    {
        enableWeaponTrail = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        footmanController = gameObject.GetComponent<FootmanController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider otherCol)
    {
        if (otherCol.CompareTag("Enemy"))
        {
            Zombie enemy = otherCol.GetComponent<Zombie>();
            Animator enemyAnimator = otherCol.GetComponent<Animator>();
            Vector3 knockbackDirection = (otherCol.transform.position - transform.position).normalized;

            if (kickHitBox.enabled)
            {
                enemy.TakeDamage(footmanController.lightAttackDamage, knockbackDirection, 4.5f);
                CameraShaker.Instance.Shake(0.7f, 10.0f, 0.2f);
                StopMotion.Instance.PlayStopMotion(animator, 0.15f);
                StopMotion.Instance.PlayStopMotion(enemyAnimator, 0.15f);

                if (enableHitEffect)
                {
                    Vector3 closestPoint = Physics.ClosestPoint(enemy.transform.position + effectOffset, kickHitBox, kickHitBox.transform.position, kickHitBox.transform.rotation);
                    GameObject hitEffect = Instantiate(kickHitEffect, closestPoint, Quaternion.identity);
                    hitEffect.transform.LookAt(kickHitBox.transform);
                }
            }
            else if (slashHitBox.enabled)
            {
                enemy.TakeDamage(footmanController.heavyAttackDamage, knockbackDirection, 3.0f);
                CameraShaker.Instance.Shake(2.0f, 1.0f, 0.25f);
                StopMotion.Instance.PlayStopMotion(animator, 0.2f);
                StopMotion.Instance.PlayStopMotion(enemyAnimator, 0.2f);

                if (enableHitEffect)
                {
                    Vector3 closestPoint = Physics.ClosestPoint(enemy.transform.position + effectOffset, slashHitBox, slashHitBox.transform.position, slashHitBox.transform.rotation);
                    GameObject hitEffect = Instantiate(slashHitEffect, closestPoint, Quaternion.identity);
                    hitEffect.transform.LookAt(slashHitBox.transform);
                }
            }
           
        }
    }

    void OnKickStart()
    {
        kickHitBox.enabled = true;
    }

    void OnKickEnd()
    {
        kickHitBox.enabled = false;
    }

    void OnSlashStart()
    {
        slashHitBox.enabled = true;
        if (enableWeaponTrail)
        {
            weaponTrail.Play();
        }
    }

    void OnSlashEnd()
    {
        slashHitBox.enabled = false;
        if (enableWeaponTrail)
        {
            weaponTrail.Stop();
        }
    }
}
