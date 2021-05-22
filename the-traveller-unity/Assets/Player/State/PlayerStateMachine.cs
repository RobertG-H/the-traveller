using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateTimer))]
public class PlayerStateMachine : MonoBehaviour
{
    PlayerState state;
    PlayerController player;
    [SerializeField] bool isDebug;
    StateTimer stateTimer;
    PlayerCooldowns cooldowns;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        state = new IdleState(player);
        stateTimer = GetComponent<StateTimer>();
        cooldowns = GetComponent<PlayerCooldowns>();
    }

    void OnGUI()
    {
        if (!isDebug) return;
        GUI.Label(new Rect(10, 50, 150, 20), string.Format("State: {0}", state));
    }

    public void HandleInput()
    {
        CheckNewState(state.HandleInput());
    }
    void Update()
    {
        CheckNewState(state.Update());
    }

    void CheckNewState(PlayerState newState)
    {
        if (newState == null || newState.ToString() == state.ToString())
            return;

        if (cooldowns.IsOnCooldown(newState.ToString())) return;

        state.StateExit();
        cooldowns.StartCooldown(state.ToString());
        state = newState;
        state.StateEnter();
    }

    public void ForceEnterState(PlayerState newState)
    {
        CheckNewState(newState);
    }

    public StateTimer GetStateTimer()
    {
        return stateTimer;
    }
}
