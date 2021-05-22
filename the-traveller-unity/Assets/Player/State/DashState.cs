using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerState
{
    float duration = 0.2f;
    float dashSpeed = 30f;
    public DashState(PlayerController player)
    {
        this.player = player;
    }
    public override PlayerState Update()
    {
        return null;
    }
    public override PlayerState HandleInput()
    {
        return null;
    }

    public override void StateEnter()
    {
        player.GetAnimations().SetTrigger("Dash");
        player.GetStateMachine().GetStateTimer().AddTimer(new StateTimerCallbackDelegate(Complete), duration);
        player.GetPhysics().StartDash(new Vector2(player.iHorz, player.iVert).normalized * dashSpeed);
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
        player.GetStateMachine().GetStateTimer().StopTimers();
        player.GetPhysics().StopDash();
    }

    private void Complete()
    {
        player.GetStateMachine().ForceEnterState(new IdleState(player));
    }
}
