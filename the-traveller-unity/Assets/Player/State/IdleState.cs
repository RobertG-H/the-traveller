using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player)
    {
        this.player = player;
    }
    public override PlayerState Update()
    {
        return CheckMoveStates();
    }
    public override PlayerState HandleInput()
    {
        PlayerState newState = CheckAbilityStates();
        if (newState != null) return newState;
        newState = CheckMoveStates();
        if (newState != null) return newState;
        return CheckIdleStates();
    }

    public override void StateEnter()
    {
        player.GetAnimations().SetTrigger("Idle");
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
    }
}
