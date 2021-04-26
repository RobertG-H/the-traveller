using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float iHorz = 0;
    public float iVert = 0;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        iHorz = context.ReadValue<float>();
        animator.SetFloat("Horizontal", iHorz);
        animator.SetFloat("Speed", Mathf.Abs(iHorz));
    }
    public void OnVertical(InputAction.CallbackContext context)
    {
        iVert = context.ReadValue<float>();
        animator.SetFloat("Vertical", iVert);
        animator.SetFloat("Speed", Mathf.Abs(iVert));

    }
}
