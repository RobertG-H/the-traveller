using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] GameObject destroyParticle;
    [SerializeField] int damage;
    [SerializeField] float forceMag;
    public bool damageBarrier = false;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (other.CompareTag("Barrier") && damageBarrier))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable.IsDamageable())
            {
                Vector2 forceDir = (other.transform.position - this.transform.position).normalized;
                // Debug.DrawLine(this.transform.position, other.transform.position, Color.magenta, 5f);
                damageable.TakeDamage(damage, forceDir * forceMag);
            }
            if (objectToDestroy)
            {
                if (destroyParticle)
                {
                    Instantiate(destroyParticle, transform.position, Quaternion.identity);
                }
                Destroy(objectToDestroy);

            }
        }
    }
}
