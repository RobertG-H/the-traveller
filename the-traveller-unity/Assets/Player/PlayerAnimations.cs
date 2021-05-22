using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    [SerializeField] PlayerController player;
    public bool isFlashing;
    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFlashing = false;
    }

    void Update()
    {
        if (!player.iDash && !player.iWorldToggle) CheckFacingDirection();
        CheckFlashing();
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

    void CheckFlashing()
    {
        if (!isFlashing)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = 1;
            spriteRenderer.color = newColor;
            CancelInvoke();
            return;
        }
        InvokeRepeating("WaitAndFlash", 0, 0.2f);
    }

    void WaitAndFlash()
    {
        Color newColor = spriteRenderer.color;
        if (newColor.a == 0)
        {
            newColor.a = 1;
            spriteRenderer.color = newColor;
        }
        else
        {
            newColor.a = 0;
            spriteRenderer.color = newColor;
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
