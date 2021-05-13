using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate void StateTimerCallbackDelegate();
public class StateTimer : MonoBehaviour
{
    private List<IEnumerator> callbackTimers;
    void Awake()
    {
        callbackTimers = new List<IEnumerator>();
    }


    public void AddTimer(StateTimerCallbackDelegate callback, float duration)
    {
        IEnumerator callbackTimer = CallbackTimer(callback, duration);
        StartCoroutine(callbackTimer);
        callbackTimers.Add(callbackTimer);
    }

    //Timer that does not get cleared when StopTimers() is called
    public void CreateStandaloneTimer(StateTimerCallbackDelegate callback, float duration)
    {
        IEnumerator callbackTimer = CallbackTimer(callback, duration);
        StartCoroutine(callbackTimer);
    }

    IEnumerator CallbackTimer(StateTimerCallbackDelegate callback, float duration)
    {
        yield return new WaitForSeconds(duration);
        callback();
    }

    public void StopTimers()
    {
        foreach (IEnumerator callbackTimer in callbackTimers)
        {
            StopCoroutine(callbackTimer);
        }
        callbackTimers.Clear();
    }
}
