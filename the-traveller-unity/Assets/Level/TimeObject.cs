
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
        canReceiveTimeEnergy = true;
    }

    float IReceiveTimeEnergy.GetRequiredTimeEnergy()
    {
        return requiredTimeEnergy;
    }
    void IReceiveTimeEnergy.ReceiveTimeEnergy()
    {
        GoToFuture();
    }
    bool IReceiveTimeEnergy.CanReceiveTimeEnergy()
    {
        return canReceiveTimeEnergy;
    }

    public void ForceGoToPast()
    {
        GoToPast();
    }

    void GoToFuture()
    {
        canReceiveTimeEnergy = false;
        pastState.SetActive(false);
        isPastState = false;
        presentState.SetActive(true);
        isPresentState = true;
    }

    void GoToPast()
    {
        pastState.SetActive(true);
        isPastState = true;
        presentState.SetActive(false);
        isPresentState = false;
    }

}