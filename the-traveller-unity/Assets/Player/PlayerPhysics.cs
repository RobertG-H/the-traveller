using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class LayerMaskConfig
{
    public static int DEFAULT;
    public static void initConfig()
    {
        //Layers that the player should ignore
        DEFAULT = 1 << LayerMask.NameToLayer("Player");
        DEFAULT += 1 << LayerMask.NameToLayer("Ignore Raycast");
        DEFAULT += 1 << LayerMask.NameToLayer("Hitbox");
        DEFAULT += 1 << LayerMask.NameToLayer("Cursor");


        DEFAULT = ~DEFAULT;
    }
}

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] PlayerController p;
    [SerializeField] BoxCollider2D ECB;

    Rigidbody2D rb;

    #region Local State Variables
    Vector2 velocity = Vector2.zero;
    // Vector2 facingDirection = Vector2.right;
    bool ignoreFriction = false;

    #endregion


    #region Physics Constants
    [Header("Physics Constants")]
    [SerializeField] float WALK_MAX_SPEED;
    [SerializeField] float WALK_ACCEL;
    [SerializeField] float FRICTION_GROUND;

    #endregion

    #region Raycasting variables
    [Header("Raycasting")]
    [SerializeField] float SKIN_WIDTH;
    [SerializeField] int HORIZONTAL_RAY_COUNT;
    [SerializeField] int VERTICAL_RAY_COUNT;
    public RaycastOrigins raycastOrigins;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    #endregion

    #region Debug
    [Header("Debug")]
    [SerializeField] bool isDebug = false;

    #endregion

    private int decimals = 4;

    void Awake()
    {
        LayerMaskConfig.initConfig();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        CalculateRaySpacing();
    }

    void OnGUI()
    {
        if (!isDebug) return;
        GUI.Label(new Rect(10, 10, 200, 20), string.Format("Velocity: {0}", velocity));
    }

    void FixedUpdate()
    {
        if (p.isDead) return;
        if (rb.isKinematic)
        {
            if (!ignoreFriction) ApplyFriction();
            Vector2 displacement = velocity * Time.fixedDeltaTime;
            ApplyMovement(displacement);
        }
    }


    #region Inherent movement method 
    void ApplyFriction()
    {
        if (velocity.magnitude == 0) return;

        Vector2 velocityDeltaFromFriction = Time.fixedDeltaTime * velocity.normalized * -1;

        velocityDeltaFromFriction *= FRICTION_GROUND;

        // Ensure friction never adds so much negative force it moves the palyer backwards
        if (velocityDeltaFromFriction.magnitude > velocity.magnitude)
        {
            velocityDeltaFromFriction = velocity * -1;
        }

        AddVel(velocityDeltaFromFriction);
    }

    private void ApplyMovement(Vector2 displacement)
    {
        UpdateRaycastOrigins();
        HorizontalCollisions(ref displacement);
        VerticalCollisions(ref displacement);
        rb.MovePosition(rb.position + new Vector2((float)System.Math.Round(displacement.x, decimals), (float)System.Math.Round(displacement.y, decimals)));
    }
    #endregion

    #region Public movement methods
    public void Walk(Vector2 input, float deltaTime)
    {
        AddVel(WALK_ACCEL * input.normalized * deltaTime);
        if (Mathf.Abs(velocity.magnitude) > WALK_MAX_SPEED)
        {
            SetVel(WALK_MAX_SPEED * input.normalized);
        }
    }

    public void HitStun(Vector2 hitStunVector)
    {
        rb.AddForce(hitStunVector, ForceMode2D.Impulse);
    }

    public void StartDash(Vector2 dashVector)
    {
        SetVel(dashVector);
        ignoreFriction = true;
    }

    public void StopDash()
    {
        SetVel(Vector2.zero);
        ignoreFriction = false;
    }

    #endregion

    #region Raycasting Methods
    void HorizontalCollisions(ref Vector2 displacement)
    {
        float rayLength = Mathf.Abs(displacement.x) + SKIN_WIDTH;
        float directionX = Mathf.Sign(displacement.x);

        for (int i = 0; i < HORIZONTAL_RAY_COUNT; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, LayerMaskConfig.DEFAULT);
            if (hit)
            {
                displacement.x = (hit.distance == 0) ? 0 : (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;
                SetVel(x: 0);
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            }
            else
            {
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.yellow);
            }
        }
    }
    void VerticalCollisions(ref Vector2 displacement)
    {
        float rayLength = Mathf.Abs(displacement.y) + SKIN_WIDTH;
        float directionY = Mathf.Sign(displacement.y);

        for (int i = 0; i < HORIZONTAL_RAY_COUNT; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + displacement.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, LayerMaskConfig.DEFAULT);

            if (hit)
            {
                displacement.y = (hit.distance == 0) ? 0 : (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;
                SetVel(y: 0);
                Debug.DrawRay(rayOrigin, Vector2.right * directionY * rayLength, Color.red);
            }
            else
            {
                Debug.DrawRay(rayOrigin, Vector2.right * directionY * rayLength, Color.yellow);
            }
        }
    }
    void CalculateRaySpacing()
    {
        Bounds bounds = ECB.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        HORIZONTAL_RAY_COUNT = Mathf.Clamp(HORIZONTAL_RAY_COUNT, 2, int.MaxValue);
        VERTICAL_RAY_COUNT = Mathf.Clamp(VERTICAL_RAY_COUNT, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (HORIZONTAL_RAY_COUNT - 1);
        verticalRaySpacing = bounds.size.x / (VERTICAL_RAY_COUNT - 1);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = ECB.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }


    #endregion

    #region Getters Setters and Public Info Methods
    public void AddVel(float x = 0, float y = 0)
    {
        velocity = new Vector2(velocity.x + x, velocity.y + y);
    }
    public void AddVel(Vector2 vel)
    {
        velocity += vel;
    }
    public void SetVel(float? x = null, float? y = null)
    {
        if (x != null && y != null)
        {
            velocity = new Vector2(x.Value, y.Value);
        }

        else if (x != null && y == null)
        {
            velocity = new Vector2(x.Value, velocity.y);
        }
        else if (y != null && x == null)
        {
            velocity = new Vector2(velocity.x, y.Value);
        }
    }
    public void SetVel(Vector2 vel)
    {
        velocity = vel;
    }

    public Vector2 GetVel()
    {
        return velocity;
    }

    public void SetDynamic()
    {
        rb.velocity = Vector2.zero;
        SetVel(Vector2.zero);
        rb.isKinematic = false;
    }
    public void SetKinematic()
    {
        rb.velocity = Vector2.zero;
        SetVel(Vector2.zero);
        rb.isKinematic = true;
    }
    #endregion
}
