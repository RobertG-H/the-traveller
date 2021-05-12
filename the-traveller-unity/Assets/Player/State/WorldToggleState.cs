using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToggleState : PlayerState
{
    bool success = false;
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
        player.GetAnimations().SetTrigger("WorldToggling");
    }

    public override void StateExit()
    {
        player.GetAnimations().ResetAnimParameters();
        if (success)
        {
            // player
        }
        else
        {

        }
    }
}
