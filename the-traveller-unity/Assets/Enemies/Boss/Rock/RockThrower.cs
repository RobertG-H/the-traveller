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

    void OnEnable()
    {
        Invoke("ThrowRock", Random.Range(timeToThrowMin, timeToThrowMax));
    }
    void ThrowRock()
    {
        if (!isActiveAndEnabled) return;
        float angleToMove = GetThrowDirection();
        Debug.Log(angleToMove);

        GameObject newRock = Instantiate(RockPrefab, transform.position, Quaternion.Euler(0, 0, 180 - angleToMove));
        newRock.transform.parent = this.transform;
        // Queue itself
        Invoke("ThrowRock", Random.Range(timeToThrowMin, timeToThrowMax));
    }

    float GetThrowDirection()
    {
        Vector3 targetPos = player.transform.position + (Vector3)player.GetPhysics().GetVel() * leadTime;
        float h = (transform.position - targetPos).magnitude;
        float a = (transform.position - targetPos).y;
        // aim to the right
        if (transform.position.x < targetPos.x) return Mathf.Acos(a / h) * Mathf.Rad2Deg * -1;

        //aim to the left
        return Mathf.Acos(a / h) * Mathf.Rad2Deg;
    }
}
