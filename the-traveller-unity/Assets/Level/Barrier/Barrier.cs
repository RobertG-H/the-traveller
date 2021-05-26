using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    TimeObject baseTimeObject;

    void OnBreak()
    {
        baseTimeObject.ForceGoToPast();
    }
}
