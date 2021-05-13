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

    void Awake()
    {
        player = GetComponent<PlayerController>();
        state = new IdleState(player);
        stateTimer = GetComponent<StateTimer>();
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

        state.StateExit();
        state = newState;
        state.StateEnter();
    }

    public void ForceExitState(PlayerState newState)
    {
        CheckNewState(newState);
    }

    public StateTimer GetStateTimer()
    {
        return stateTimer;
    }
}
