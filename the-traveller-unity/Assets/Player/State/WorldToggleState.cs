using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToggleState : PlayerState
{
    bool success = false;
    float duration = 1f;
    public WorldToggleState(PlayerController player)
    {
        this.player = player;
    }
    public override PlayerState Update()
    {
        return null;
    }
    public override PlayerState HandleInput()
    {
        if (!player.iWorldToggle)
        {
            return new IdleState(player);
        }
        return null;
    }

    public override void StateEnter()
    {
        // Start timer
        player.SetWorldToggleParticles(true);

        player.GetAnimations().SetTrigger("WorldToggling");
        player.GetStateMachine().GetStateTimer().AddTimer(new StateTimerCallbackDelegate(Complete), duration);
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
        player.SetWorldToggleParticles(false);

        if (success)
        {
            player.WorldToggle();
            Debug.Log("world toggle complete!");
        }
        else
        {
            Debug.Log("World toggle failed...");
            player.GetStateMachine().GetStateTimer().StopTimers();
        }
    }

    private void Complete()
    {
        success = true;
        player.GetStateMachine().ForceEnterState(new IdleState(player));
    }
}
