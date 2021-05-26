
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour, IReceiveTimeEnergy
{
    public float requiredTimeEnergy;
    public bool canReceiveTimeEnergy;

    public GameObject pastState;
    public bool isPastState;

    public GameObject presentState;
    public bool isPresentState;

    void Awake()
    {
        if (isPastState == isPresentState)
        {
            Debug.LogError("States are equal. Should not happen");
        }
        pastState.SetActive(isPastState);
        presentState.SetActive(isPresentState);
    }

    float IReceiveTimeEnergy.GetRequiredTimeEnergy()
    {
        return requiredTimeEnergy;
    }
    void IReceiveTimeEnergy.ReceiveTimeEnergy()
    {

    }
    bool IReceiveTimeEnergy.CanReceiveTimeEnergy()
    {
        return canReceiveTimeEnergy;
    }

}