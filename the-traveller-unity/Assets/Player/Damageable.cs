using UnityEngine;
interface Damageable
{
    void TakeDamage(int damage);
    void TakeDamage(int damage, Vector2 force);
    bool IsDamageable();
}