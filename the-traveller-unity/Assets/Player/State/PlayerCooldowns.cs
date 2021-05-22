using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilityCooldown
{
    public bool onCooldown;
    public float duration;

    public AbilityCooldown(float duration)
    {
        this.duration = duration;
        onCooldown = false;
    }
}

public delegate void CooldownCompleteCallbackDelegate(string stateName);
public class PlayerCooldowns : MonoBehaviour
{
    Dictionary<string, AbilityCooldown> abilityCooldowns;
    private List<IEnumerator> callbackTimers;
    void Awake()
    {
        callbackTimers = new List<IEnumerator>();
        abilityCooldowns = new Dictionary<string, AbilityCooldown>();
        abilityCooldowns["DashState"] = new AbilityCooldown(1f);
        abilityCooldowns["WorldToggleState"] = new AbilityCooldown(1f);
    }

    public bool IsOnCooldown(string stateName)
    {
        AbilityCooldown value;
        if (abilityCooldowns.TryGetValue(stateName, out value))
        {
            return value.onCooldown;
        }
        else
        {
            return false;
        }
    }

    public void StartCooldown(string stateName)
    {
        AbilityCooldown value;
        if (abilityCooldowns.TryGetValue(stateName, out value))
        {
            CooldownCompleteCallbackDelegate callback = new CooldownCompleteCallbackDelegate(CooldownComplete);
            IEnumerator callbackTimer = CallbackTimer(callback, abilityCooldowns[stateName].duration, stateName);
            StartCoroutine(callbackTimer);
            callbackTimers.Add(callbackTimer);
            abilityCooldowns[stateName].onCooldown = true;
        }
    }

    IEnumerator CallbackTimer(CooldownCompleteCallbackDelegate callback, float duration, string stateName)
    {
        yield return new WaitForSeconds(duration);
        callback(stateName);
    }

    void CooldownComplete(string stateName)
    {
        AbilityCooldown value;

        if (abilityCooldowns.TryGetValue(stateName, out value))
        {
            abilityCooldowns[stateName].onCooldown = false;
        }
    }
}