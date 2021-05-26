using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrower : MonoBehaviour
{
    public GameObject RockPrefab;
    [SerializeField] GameObject playerObj;
    PlayerController player;
    [SerializeField] float timeToThrowMin;
    [SerializeField] float timeToThrowMax;
    [SerializeField] float leadTime;

    void Awake()
    {
        player = playerObj.GetComponent<PlayerController>();
    }

    void Start()
    {
        Invoke("ThrowRock", Random.Range(timeToThrowMin, timeToThrowMax));

    }



    void ThrowRock()
    {
        // Throw rock
        Instantiate(RockPrefab, transform.position, Quaternion.identity);

        // Queue itself
        Invoke("ThrowRock", Random.Range(timeToThrowMin, timeToThrowMax));

    }

    void GetThrowDirection()
    {
        player.GetPhysics().GetVel();
        // Vector2 newPos
    }
}
