using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState state;
    [SerializeField] bool isDebug;


    void Awake()
    {
        state = new IdleState(GetComponent<PlayerController>());
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
}
