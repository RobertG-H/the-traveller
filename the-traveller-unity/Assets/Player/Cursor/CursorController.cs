using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] PlayerController player;
    public GameObject energyTextObj;
    public TextMeshProUGUI energyTextMesh;
    IReceiveTimeEnergy currentHover;
    float minDistance = 9f;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        energyTextObj.SetActive(false);
    }

    void Update()
    {
        Cursor.visible = false;

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(player.mousePosition);
        rb.MovePosition(cursorPos);

        if (currentHover != null)
        {
            CheckClick();
            CheckCloseEnough();
        }
        else
        {
            OnIdle();
        }
    }

    void CheckClick()
    {

        if (player.iClick && currentHover != null && currentHover.CanReceiveTimeEnergy())
        {
            if (IsCloseEnough() && player.GetTimeEnergy() >= currentHover.GetRequiredTimeEnergy())
            {
                player.SubtractTimeEnergy(currentHover.GetRequiredTimeEnergy());
                currentHover.ReceiveTimeEnergy();
                OnHoverCannotGiveEnergy();
            }

        }
    }

    void CheckCloseEnough()
    {
        if (!IsCloseEnough())
        {
            OnHoverTooFar();
            return;
        }
        UpdateOnHoverStatus();
    }

    void UpdateOnHoverStatus()
    {
        if (currentHover == null) return;
        if (currentHover.CanReceiveTimeEnergy())
        {
            OnHoverCanGiveEnergy();
        }
        else
        {
            OnHoverCannotGiveEnergy();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent)
        {
            if (other.transform.parent.CompareTag("TimeObject"))
            {
                currentHover = other.transform.parent.GetComponent<IReceiveTimeEnergy>();
                if (!IsCloseEnough())
                {
                    OnHoverTooFar();
                    return;
                }
                UpdateOnHoverStatus();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent)
        {
            if (other.transform.parent.CompareTag("TimeObject"))
            {
                OnIdle();
            }
        }
    }

    bool IsCloseEnough()
    {
        return (player.transform.position - transform.position).magnitude <= minDistance;
    }

    void OnHoverTooFar()
    {
        energyTextObj.SetActive(true);
        energyTextMesh.text = string.Format("Go closer");
        TriggerAnimation("not-available");
    }

    void OnHoverCanGiveEnergy()
    {
        if (player.GetTimeEnergy() < currentHover.GetRequiredTimeEnergy())
        {
            energyTextObj.SetActive(true);
            energyTextMesh.text = string.Format("Requires {0} Time Energy", currentHover.GetRequiredTimeEnergy());
            TriggerAnimation("not-available");
        }
        else
        {
            energyTextObj.SetActive(true);
            energyTextMesh.text = string.Format("Transfer {0} Time Energy?", currentHover.GetRequiredTimeEnergy());
            TriggerAnimation("available");
        }

    }

    void OnHoverCannotGiveEnergy()
    {
        energyTextObj.SetActive(true);
        energyTextMesh.text = "Unable to transfer";
        TriggerAnimation("not-available");
    }

    void OnIdle()
    {
        energyTextObj.SetActive(false);
        currentHover = null;
        TriggerAnimation("idle");
    }

    void TriggerAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

}
