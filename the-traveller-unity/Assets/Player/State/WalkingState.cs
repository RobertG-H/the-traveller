using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : PlayerState
{
    public WalkingState(PlayerController player)
    {
        this.player = player;
    }
    public override PlayerState Update()
    {
        player.Walk();
        player.GetAnimations().SetFloat("Horizontal", player.iHorz);
        player.GetAnimations().SetFloat("Vertical", player.iVert);
        return null;
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
        player.GetAnimations().SetTrigger("Walking");
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
    }
}
