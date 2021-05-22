using UnityEngine;
interface Damageable
{
    void TakeDamage(float damage);
    void TakeDamage(float damage, Vector2 force);
    bool IsDamageable();
}