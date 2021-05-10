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
