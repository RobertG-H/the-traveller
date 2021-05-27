using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, Damageable
{
    public TimeObject baseTimeObject;

    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        currentHealth = maxHealth;
    }

    void OnFix()
    {
        Initialize();
    }

    void OnBreak()
    {
        baseTimeObject.ForceGoToPast();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TAKE damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnBreak();
        }
    }

    public void TakeDamage(int damage, Vector2 force)
    {
        TakeDamage(damage);
    }


    public bool IsDamageable()
    {
        return baseTimeObject.isPresentState;
    }
}
