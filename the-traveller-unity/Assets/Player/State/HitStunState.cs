using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStunState : PlayerState
{
    Vector2 force;
    float duration = 0.3f;
    public HitStunState(PlayerController player, Vector2 force)
    {
        this.player = player;
        this.force = force;
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
        player.GetAnimations().SetTrigger("HitStun");
        player.GetPhysics().SetDynamic();
        player.GetStateMachine().GetStateTimer().AddTimer(new StateTimerCallbackDelegate(Complete), duration);
        player.GetPhysics().HitStun(force);
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
    }

    void Complete()
    {
        player.GetStateMachine().ForceEnterState(new IdleState(player));
        player.GetPhysics().SetKinematic();

    }
}
