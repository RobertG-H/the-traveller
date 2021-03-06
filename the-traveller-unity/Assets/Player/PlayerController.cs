using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, Damageable
{
    [ReadOnly] public float iHorz = 0;
    [ReadOnly] public float iVert = 0;
    [ReadOnly] public bool iWorldToggle = false;
    [SerializeField] ParticleSystem worldToggleParticles;
    [ReadOnly] public bool iDash = false;
    [ReadOnly] public bool iClick = false;
    [ReadOnly] public bool iInteracting = false;
    [ReadOnly] public Vector2 mousePosition = Vector2.zero;
    [SerializeField] int maxHealth;
    [SerializeField, ReadOnly] int health;
    [SerializeField] float maxTimeEnergy;
    [SerializeField, ReadOnly] float timeEnergy;
    bool isDamageable;
    public bool isDead = false;
    PlayerPhysics physics;
    PlayerStateMachine stateMachine;
    [SerializeField] PlayerAnimations animations;
    [SerializeField] HUDController hudController;
    public GameObject dashHitboxObject;

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
        worldToggleParticles.gameObject.SetActive(false);


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

    public void OnToggleHelp(InputAction.CallbackContext context)
    {
        hudController.ToggleHelpDetails();
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            iClick = true;
        }
        else if (context.canceled)
        {
            iClick = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            iInteracting = true;
        }
        else if (context.canceled)
        {
            iInteracting = false;
        }
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

    public float GetTimeEnergy()
    {
        return timeEnergy;
    }

    public void SubtractTimeEnergy(float amount)
    {
        timeEnergy -= amount;
        hudController.SetCurrentTimeEnergy(timeEnergy);
    }

    public void SetWorldToggleParticles(bool status)
    {
        if (status)
        {
            worldToggleParticles.gameObject.SetActive(true);
            worldToggleParticles.Play();
        }
        else
        {
            worldToggleParticles.gameObject.SetActive(false);
        }
    }

    #endregion

    void Damageable.TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        StartCoroutine(InvincibleTimer());
        if (health <= 0) PlayerDied();
    }

    void Damageable.TakeDamage(int damage, Vector2 force)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0) PlayerDied();
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

    void PlayerDied()
    {
        isDead = true;
        hudController.ShowDeathText();
    }




}
