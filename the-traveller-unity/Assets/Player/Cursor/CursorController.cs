using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    [SerializeField] PlayerController player;
    public GameObject energyTextObj;
    public TextMeshProUGUI energyTextMesh;
    IReceiveTimeEnergy currentHover;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimeObject"))
        {
            currentHover = other.GetComponent<IReceiveTimeEnergy>();
            if (currentHover.CanReceiveTimeEnergy())
            {
                OnHoverCanGiveEnergy();
            }
            else
            {
                OnHoverCannotGiveEnergy();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeObject"))
        {
            spriteRenderer.color = Color.white;
            energyTextObj.SetActive(false);
            currentHover = null;
        }
    }

    void OnHoverCanGiveEnergy()
    {
        spriteRenderer.color = Color.blue;
        energyTextObj.SetActive(true);
        energyTextMesh.color = Color.blue;


    }

    void OnHoverCannotGiveEnergy()
    {
        spriteRenderer.color = Color.red;
    }

}
