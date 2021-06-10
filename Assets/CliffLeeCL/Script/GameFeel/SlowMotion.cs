using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public static SlowMotion Instance { get; private set; } = null;

    public AnimationCurve attackCurve = null;
    public AnimationCurve releaseCurve = null;

    [Header("Override SlowMotion param")]
    public bool canOverrideParam = false;
    public float overrideTargetTimeScale = 0.2f;
    public float overrideAttackTime = 0.5f;
    public float overrideSustainTime = 1.0f;
    public float overrideReleaseTime = 0.5f;

    enum SlowMotionStage
    {
        Attack,
        Sustain,
        Release,
        End
    }
    SlowMotionStage stage = SlowMotionStage.End;
    const float STANDARD_TIME_SCALE = 1.0f;
    float targetTimeScale = 0.0f;
    float attackTime = 0.0f;
    float sustainTime = 0.0f;
    float releaseTime = 0.0f;
    float elapsedTime = 0.0f;

    public void PlaySlowMotion(float inTargetTimeScale, float inAttackTime, float inSustainTime, float inReleaseTime)
    {
        if (!this.enabled)
        {
            return;
        }
        targetTimeScale = canOverrideParam ? overrideTargetTimeScale : inTargetTimeScale;
        attackTime = canOverrideParam ? overrideAttackTime : inAttackTime;
        sustainTime = canOverrideParam ? overrideSustainTime : inSustainTime;
        releaseTime = canOverrideParam ? overrideReleaseTime : inReleaseTime;
        elapsedTime = 0.0f;
        stage = SlowMotionStage.Attack;
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

    void Update()
    {
        if (stage != SlowMotionStage.End)
        {
            switch (stage)
            {
                case SlowMotionStage.Attack:
                    if (elapsedTime <= attackTime)
                    {
                        float normalizedTime = elapsedTime / (attackTime + Mathf.Epsilon);
                        float delta = (STANDARD_TIME_SCALE - targetTimeScale) * attackCurve.Evaluate(normalizedTime);

                        Time.timeScale = STANDARD_TIME_SCALE - delta;
                    }
                    else
                    {
                        elapsedTime = 0;
                        stage = SlowMotionStage.Sustain;
                    }
                    break;
                case SlowMotionStage.Sustain:
                    if (elapsedTime <= sustainTime)  
                    {
                        Time.timeScale = targetTimeScale;
                    }
                    else
                    {
                        elapsedTime = 0;
                        stage = SlowMotionStage.Release;
                    }
                    break;
                case SlowMotionStage.Release:
                    if (elapsedTime <= releaseTime)  
                    {
                        float normalizedTime = elapsedTime / (releaseTime + Mathf.Epsilon);
                        float delta = (STANDARD_TIME_SCALE - targetTimeScale) * releaseCurve.Evaluate(normalizedTime);

                        Time.timeScale = STANDARD_TIME_SCALE - delta;
                    }
                    else
                    {
                        elapsedTime = 0;
                        Time.timeScale = STANDARD_TIME_SCALE;
                        stage = SlowMotionStage.End;
                    }
                    break;
            }
            elapsedTime += Time.unscaledDeltaTime;
        }
    }
}
