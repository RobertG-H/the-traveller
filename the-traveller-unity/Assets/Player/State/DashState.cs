using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerState
{
    float duration = 0.15f;
    float dashSpeed = 34f;
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
        player.dashHitboxObject.SetActive(true);

    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
        player.GetStateMachine().GetStateTimer().StopTimers();
        player.GetPhysics().StopDash();
        player.dashHitboxObject.SetActive(false);

    }

    private void Complete()
    {
        player.GetStateMachine().ForceEnterState(new IdleState(player));
    }
}
