using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField]
    PlayerController p;
    [SerializeField]
    private float speed;
    private int decimals = 4;
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(p.iHorz, p.iVert);
        ApplyMovement(velocity * Time.fixedDeltaTime * speed);
    }

    private void ApplyMovement(Vector3 displacement)
    {
        gameObject.transform.Translate(new Vector3((float)System.Math.Round(displacement.x, decimals),
        (float)System.Math.Round(displacement.y, decimals),
        (float)System.Math.Round(displacement.z, decimals)),
        relativeTo: Space.World);
    }
}
