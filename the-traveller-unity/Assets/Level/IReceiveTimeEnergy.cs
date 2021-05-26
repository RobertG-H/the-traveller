using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IReceiveTimeEnergy
{
    float GetRequiredTimeEnergy();
    void ReceiveTimeEnergy();
    bool CanReceiveTimeEnergy();
}
