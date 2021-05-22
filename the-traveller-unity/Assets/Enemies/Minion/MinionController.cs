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

public class MinionController : MonoBehaviour
{
    public bool isActive = true;
    public AIPath aiPath;
    [SerializeField] MinionAnimations animations;
    [SerializeField] GameObject fire;
    [SerializeField, ReadOnly] MinionStates currentState;
    [SerializeField] float timeToToggle = 1.5f;

    bool damageTimerOn;
    bool needToDamageToggle;
    bool isInDamageMode;

    void Awake()
    {
        aiPath.isStopped = true;
        currentState = MinionStates.Idle;
        isInDamageMode = false;
        needToDamageToggle = false;
        damageTimerOn = false;
        fire.SetActive(false);
    }
    void ChangeState(MinionStates newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        animations.ChangeState(newState);
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
        }
        else
        {
            isInDamageMode = true;
            fire.SetActive(true);
        }
    }

    void OnDisable()
    {
        // damageTimerOn = false;
        needToDamageToggle = false;
        fire.SetActive(false);
        isInDamageMode = false;
    }

}
