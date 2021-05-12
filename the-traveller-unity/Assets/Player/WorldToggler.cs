using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToggler : MonoBehaviour
{
    [SerializeField] GameObject futureRealm;
    [SerializeField] GameObject fireRealm;

    public void ToggleWorlds()
    {
        if (futureRealm.activeSelf)
        {
            futureRealm.SetActive(false);
            fireRealm.SetActive(true);
        }
        else
        {
            futureRealm.SetActive(true);
            fireRealm.SetActive(false);
        }
    }
}
