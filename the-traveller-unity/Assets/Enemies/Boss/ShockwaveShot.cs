using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveShot : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb.velocity = transform.right * moveSpeed;
    }
}
