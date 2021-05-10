using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float iHorz = 0;
    public float iVert = 0;

    private Animator animator;

    PlayerPhysics physics;

    void Awake()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<PlayerPhysics>();
    }

    void Update()
    {
        //todo move this to state
        physics.Walk(new Vector2(iHorz, iVert), Time.deltaTime);
        animator.SetFloat("Speed", new Vector2(iHorz, iVert).magnitude);

    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        iHorz = context.ReadValue<float>();
        animator.SetFloat("Horizontal", iHorz);
    }
    public void OnVertical(InputAction.CallbackContext context)
    {
        iVert = context.ReadValue<float>();
        animator.SetFloat("Vertical", iVert);
    }
}
