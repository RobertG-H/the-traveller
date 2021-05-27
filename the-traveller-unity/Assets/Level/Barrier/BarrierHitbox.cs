using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitbox : MonoBehaviour, Damageable
{
    public Barrier parent;

    bool Damageable.IsDamageable()
    {
        return parent.IsDamageable();
    }

    void Damageable.TakeDamage(int damage)
    {
        parent.TakeDamage(damage);
    }

    void Damageable.TakeDamage(int damage, Vector2 force)
    {
        parent.TakeDamage(damage);
        Debug.Log("TAKE damage");

    }
}
