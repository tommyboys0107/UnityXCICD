using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMotion : MonoBehaviour
{
    public static StopMotion Instance { get; private set; } = null;

    [Header("Override StopMotion param")]
    public bool canOverrideParam = false;
    public float overrideStopTime = 0.3f;

    public void PlayStopMotion(Animator animator, float stopTime = 0.2f)
    {
        if (!this.enabled)
        {
            return;
        }

        StartCoroutine(PerformStopMotion(animator, canOverrideParam ? overrideStopTime : stopTime));
    }

    IEnumerator PerformStopMotion(Animator animator, float stopTime)
    {
        animator.speed = 0.0f;
        yield return new WaitForSeconds(stopTime);
        animator.speed = 1.0f;
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
