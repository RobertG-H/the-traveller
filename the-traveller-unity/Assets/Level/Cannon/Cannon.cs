using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public PlayerController player;
    public TimeObject parent;
    public GameObject cannonShot;
    public bool isFuture;
    bool isShooting = false;
    private float minDistance = 3f;
    public bool doShootLeft = false;

    void OnEnable()
    {
        CancelInvoke();
        ResetShooting();
    }
    void Update()
    {
        if (isShooting || !CanShoot()) return;
        if (player.iInteracting && IsPlayerCloseEnough())
        {
            Fire();
        }
    }

    void Fire()
    {
        isShooting = true;
        Invoke("ResetShooting", 1f);
        GameObject newShot = Instantiate(cannonShot, transform.position, Quaternion.identity);
        if (doShootLeft)
        {
            newShot.transform.localRotation = Quaternion.Euler(0, 0, -180);
            newShot.GetComponent<CannonShot>().rb.velocity = newShot.GetComponent<CannonShot>().moveSpeed * transform.right * -1;
        }
        newShot.transform.parent = transform;
        if (isFuture)
        {
            parent.ForceGoToPast();
        }
    }

    bool IsPlayerCloseEnough()
    {
        return (player.transform.position - transform.position).magnitude <= minDistance;
    }

    void ResetShooting()
    {
        isShooting = false;
    }

    bool CanShoot()
    {
        if (!isFuture) return true;
        return parent.isPresentState;
    }
}
