using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Damageable
{
    [ReadOnly] public float iHorz = 0;
    [ReadOnly] public float iVert = 0;
    [ReadOnly] public bool iWorldToggle = false;
    [ReadOnly] public bool iDash = false;
    [SerializeField] float maxHealth;
    [SerializeField, ReadOnly] float health;
    bool isDamageable;
    PlayerPhysics physics;
    PlayerStateMachine stateMachine;
    [SerializeField] PlayerAnimations animations;
    WorldToggler worldToggler;

    [Header("Debug")]
    [SerializeField] bool isDebug;

    void Awake()
    {
        physics = GetComponent<PlayerPhysics>();
        stateMachine = GetComponent<PlayerStateMachine>();
        worldToggler = GetComponent<WorldToggler>();
        Initialize();
    }

    void Initialize()
    {
        health = maxHealth;
        isDamageable = true;
    }

    #region Player Actions

    public void Walk()
    {
        Vector2 displacement = new Vector2(iHorz, iVert);
        physics.Walk(displacement, Time.deltaTime);
    }

    public void WorldToggle()
    {
        worldToggler.ToggleWorlds();
    }

    #endregion

    #region Input

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        iHorz = context.ReadValue<float>();
        stateMachine.HandleInput();
    }
    public void OnVertical(InputAction.CallbackContext context)
    {
        iVert = context.ReadValue<float>();
        stateMachine.HandleInput();
    }

    public void OnWorldToggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            iWorldToggle = true;
        }
        else if (context.canceled)
        {
            iWorldToggle = false;
        }
        stateMachine.HandleInput();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            iDash = true;
        }
        else if (context.canceled)
        {
            iDash = false;
        }
        stateMachine.HandleInput();
    }

    #endregion

    #region Getters and Setters
    public PlayerAnimations GetAnimations()
    {
        return animations;
    }

    public PlayerStateMachine GetStateMachine()
    {
        return stateMachine;
    }

    public PlayerPhysics GetPhysics()
    {
        return physics;
    }

    #endregion

    void Damageable.TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(InvincibleTimer());
    }

    void Damageable.TakeDamage(float damage, Vector2 force)
    {
        health -= damage;
        stateMachine.ForceEnterState(new HitStunState(this, force));
        StartCoroutine(InvincibleTimer());
    }

    bool Damageable.IsDamageable()
    {
        return isDamageable;
    }

    private IEnumerator InvincibleTimer()
    {
        animations.isFlashing = true;
        while (true)
        {
            yield return new WaitForSeconds(0.8f);
            isDamageable = true;
            animations.isFlashing = false;
        }
    }


}
