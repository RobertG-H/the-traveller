using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public enum MinionStates
{
    Idle,
    Walking,
    DamageToggling,
}

public class MinionController : MonoBehaviour, IGiveTimeEnergy
{
    public bool isActive = true;
    public AIPath aiPath;
    [SerializeField] MinionAnimations animations;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject hitBox;
    [SerializeField, ReadOnly] MinionStates currentState;
    [SerializeField] float timeToToggleMin = 1.1f;
    [SerializeField] float timeToToggleMax = 2.3f;
    [SerializeField] float timeEnergy = 25f;

    float timeToToggle;
    bool damageTimerOn;
    bool needToDamageToggle;
    bool isInDamageMode;
    AIDestinationSetter destinationSetter;

    void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }
    void ChangeState(MinionStates newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        animations.ChangeState(newState);
    }

    void OnEnable()
    {
        aiPath.isStopped = true;
        hitBox.SetActive(true);

        currentState = MinionStates.Idle;
        isInDamageMode = false;
        needToDamageToggle = false;
        damageTimerOn = false;
        CancelInvoke();
        timeToToggle = Random.Range(timeToToggleMin, timeToToggleMax);
        fire.SetActive(false);
    }

    #region Tasks
    [Task]
    bool IsActive() { return isActive; }

    [Task]
    bool TryStartToggle()
    {
        if (!isActive)
        {
            isInDamageMode = false;
            damageTimerOn = false;
            return false;
        }
        if (damageTimerOn) return true;
        InvokeRepeating("SetNeedToDamageToggle", timeToToggle, timeToToggle);
        damageTimerOn = true;
        return true;
    }

    [Task]
    bool NeedsToDamageToggle()
    {
        return needToDamageToggle;
    }

    [Task]
    bool StartDamageToggle()
    {
        aiPath.isStopped = true;
        ChangeState(MinionStates.DamageToggling);
        return true;
    }

    [Task]
    bool PathToPlayer()
    {
        ChangeState(MinionStates.Walking);
        aiPath.isStopped = false;
        return true;
    }

    [Task]
    bool TogglingMode()
    {
        return true;
    }
    #endregion

    void SetNeedToDamageToggle()
    {
        if (!damageTimerOn) return;
        needToDamageToggle = true;
    }

    public void CompleteDamageToggle()
    {
        needToDamageToggle = false;
        if (isInDamageMode)
        {
            isInDamageMode = false;
            fire.SetActive(false);
            hitBox.SetActive(true);

        }
        else
        {
            isInDamageMode = true;
            fire.SetActive(true);
            hitBox.SetActive(false);
        }
    }

    public void SetPathTarget(Transform newTarget)
    {
        destinationSetter.target = newTarget;
    }

    void OnDisable()
    {
        // damageTimerOn = false;
        needToDamageToggle = false;
        fire.SetActive(false);
        isInDamageMode = false;
    }

    float IGiveTimeEnergy.GetTimeEnergy()
    {
        return timeEnergy;
    }

}
