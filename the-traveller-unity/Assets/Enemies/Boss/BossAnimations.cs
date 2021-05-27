using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    Animator anim;
    public BossController parent;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void StartShockwave()
    {
        anim.SetTrigger("Shockwave");
    }

    public void ShockwaveExplosion()
    {
        parent.DoShockwave();
    }

    public void Idle()
    {
        anim.SetTrigger("Idle");
    }
}
