using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionAnimations : MonoBehaviour
{
    public AIPath aiPath;

    [SerializeField] MinionController minionController;


    Animator anim;
    Dictionary<MinionStates, string> stateToString;

    void Awake()
    {
        anim = GetComponent<Animator>();
        stateToString = new Dictionary<MinionStates, string>();
        stateToString[MinionStates.Idle] = "Idle";
        stateToString[MinionStates.Walking] = "Movement";
        stateToString[MinionStates.DamageToggling] = "Minion_Charging";
    }
    void Update()
    {
        anim.SetFloat("Horizontal", aiPath.desiredVelocity.x);
        anim.SetFloat("Vertical", aiPath.desiredVelocity.y);

        //Flips sprite based on direction it is going
        if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void ChangeState(MinionStates newState)
    {
        anim.Play(stateToString[newState]);
    }
    public void CompleteDamageToggle()
    {
        minionController.CompleteDamageToggle();
    }
}
