using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float timeToDie = 1f;

    void Start()
    {
        Invoke("KillSelf", timeToDie);
    }

    void KillSelf()
    {
        Destroy(gameObject);
    }
}
