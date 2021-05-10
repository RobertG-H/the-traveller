using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [ReadOnly] public float iHorz = 0;
    [ReadOnly] public float iVert = 0;

    PlayerPhysics physics;

    PlayerState state;

    [Header("Debug")]
    [SerializeField] bool isDebug;

    void Awake()
    {
        physics = GetComponent<PlayerPhysics>();
        state = new IdleState(this);
    }

    void OnGUI()
    {
        if (!isDebug) return;
        GUI.Label(new Rect(10, 50, 150, 20), string.Format("State: {0}", state));
    }

    void Update()
    {
        CheckNewState(state.Update());
    }

    void HandleInput()
    {
        CheckNewState(state.HandleInput());
    }

    private void CheckNewState(PlayerState newState)
    {
        if (newState == null || newState.ToString() == state.ToString())
            return;

        state.StateExit();
        state = newState;
        state.StateEnter();
    }

    public void Walk()
    {
        Vector2 displacement = new Vector2(iHorz, iVert);
        physics.Walk(displacement, Time.deltaTime);
    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        iHorz = context.ReadValue<float>();
        HandleInput();
    }
    public void OnVertical(InputAction.CallbackContext context)
    {
        iVert = context.ReadValue<float>();
        HandleInput();
    }
}
