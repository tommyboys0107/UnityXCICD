using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public static Knockback Instance { get; private set; } = null;

    [Header("Override Knockback param")]
    public bool canOverrideParam = false;
    public float overrideForceMultiplier = 10.0f;

    public void KnockbackTarget(Rigidbody targetRigidbody, Vector3 forceDirection, float forceMultiplier = 10.0f)
    {
        if (!this.enabled)
        {
            return;
        }

        targetRigidbody.AddForce(forceDirection * (canOverrideParam ? overrideForceMultiplier : forceMultiplier), ForceMode.Impulse);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }
}
