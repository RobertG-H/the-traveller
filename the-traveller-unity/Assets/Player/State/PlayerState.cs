using UnityEngine;
public abstract class PlayerState
{

    protected PlayerController player;
    public abstract PlayerState Update();
    public abstract PlayerState HandleInput();
    public abstract void StateEnter();
    public abstract void StateExit();

    public string name;

    protected PlayerState CheckAbilityStates()
    {
        if (player.iWorldToggle)
        {
            return new WorldToggleState(player);
        }
        return null;
    }

    protected PlayerState CheckMoveStates()
    {

        if (Mathf.Abs(player.iHorz) > 0 || Mathf.Abs(player.iVert) > 0)
        {
            return new WalkingState(player);
        }
        return null;
    }

    protected PlayerState CheckIdleStates()
    {
        if (player.iHorz == 0 && player.iVert == 0)
        {
            return new IdleState(player);
        }
        return null;
    }

    public override string ToString()
    {
        return this.GetType().Name;
    }
}