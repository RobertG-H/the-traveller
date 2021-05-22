using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] int damage;
    [SerializeField] float forceMag;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable.IsDamageable())
            {
                Vector2 forceDir = (other.transform.position - this.transform.position).normalized;
                // Debug.DrawLine(this.transform.position, other.transform.position, Color.magenta, 5f);
                damageable.TakeDamage(damage, forceDir * forceMag);
            }
        }
        if (objectToDestroy) Destroy(objectToDestroy);
    }
}
