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
    [SerializeField] int maxHealth;
    [SerializeField, ReadOnly] int health;
    [SerializeField] float maxTimeEnergy;
    [SerializeField, ReadOnly] float timeEnergy;
    bool isDamageable;
    PlayerPhysics physics;
    PlayerStateMachine stateMachine;
    [SerializeField] PlayerAnimations animations;
    [SerializeField] HUDController hudController;
    [SerializeField] GameObject dashHitboxObject;

    WorldToggler worldToggler;

    [Header("Debug")]
    [SerializeField] bool isDebug;

    void Awake()
    {
        physics = GetComponent<PlayerPhysics>();
        stateMachine = GetComponent<PlayerStateMachine>();
        worldToggler = GetComponent<WorldToggler>();

    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        health = maxHealth;
        hudController.SetMaxHealth(maxHealth);
        hudController.SetCurrentHealth(health);
        hudController.SetCurrentTimeEnergy(timeEnergy);
        isDamageable = true;
        dashHitboxObject.SetActive(false);

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
            dashHitboxObject.SetActive(true);
        }
        else if (context.canceled)
        {
            iDash = false;
            dashHitboxObject.SetActive(false);
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

    void Damageable.TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(InvincibleTimer());
    }

    void Damageable.TakeDamage(int damage, Vector2 force)
    {
        health -= damage;
        hudController.SetCurrentHealth(health);
        stateMachine.ForceEnterState(new HitStunState(this, force));
        StartCoroutine(InvincibleTimer());
    }

    bool Damageable.IsDamageable()
    {
        return isDamageable;
    }

    public void GainTimeEnergy(float timeEnergy)
    {
        if (this.timeEnergy == maxTimeEnergy) return;
        if (this.timeEnergy + timeEnergy > maxTimeEnergy)
        {
            this.timeEnergy = maxTimeEnergy;
        }
        else
        {
            this.timeEnergy += timeEnergy;
        }
        hudController.SetCurrentTimeEnergy(this.timeEnergy);
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
