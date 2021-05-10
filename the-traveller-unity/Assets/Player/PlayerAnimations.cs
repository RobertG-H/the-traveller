using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    SpriteRenderer renderer;
    [SerializeField] PlayerController player;
    void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckFacingDirection();

        Vector2 displacement = new Vector2(player.iHorz, player.iVert);
        animator.SetFloat("Horizontal", displacement.x);
        animator.SetFloat("Vertical", displacement.y);
        animator.SetFloat("Speed", displacement.magnitude);


    }

    void CheckFacingDirection()
    {
        if (player.iHorz < 0)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
    }
}
