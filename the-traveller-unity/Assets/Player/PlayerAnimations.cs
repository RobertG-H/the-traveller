using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    [SerializeField] PlayerController player;
    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckFacingDirection();
    }

    void CheckFacingDirection()
    {
        if (player.iHorz < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    #region Public methods
    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void ResetAnimParameters()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
            else if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
            else if (param.type == AnimatorControllerParameterType.Float)
            {
                animator.SetFloat(param.name, 0);
            }
        }
    }

    #endregion
}
