using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] float damage;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable.IsDamageable()) damageable.TakeDamage(damage);
        }
        if (objectToDestroy) Destroy(objectToDestroy);
    }
}
