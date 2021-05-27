using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    [SerializeField] GameObject destroyParticle;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb.velocity = transform.right * moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BossHurtbox"))
        {
            BossController boss = other.transform.parent.GetComponent<BossController>();
            boss.GotShot();
            if (destroyParticle)
            {
                Instantiate(destroyParticle, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
