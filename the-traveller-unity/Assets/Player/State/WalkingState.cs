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
        return null;
    }
    public override PlayerState HandleInput()
    {
        PlayerState newState = CheckMoveStates();
        return newState != null ? newState : CheckIdleStates();
    }

    public override void StateEnter()
    {
    }

    public override void StateExit()
    {

    }
}
