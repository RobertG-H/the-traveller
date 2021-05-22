using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    public PlayerController p;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimeEnergy"))
        {
            IGiveTimeEnergy target = other.GetComponentInParent<IGiveTimeEnergy>();
            p.GainTimeEnergy(target.GetTimeEnergy());
            Destroy(other.transform.parent.gameObject);
        }
    }
}
