using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Rigidbody2D rb;
    public SpriteRenderer sr;
    public float moveSpeed;
    public List<Sprite> rockSprites;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr.sprite = rockSprites[Random.Range(0, rockSprites.Count)];
    }

    void Start()
    {
        rb.velocity = transform.up * moveSpeed;
    }

}