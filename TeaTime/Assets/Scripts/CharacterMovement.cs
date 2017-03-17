using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float JumpForce;

    [Header("GroundCheck")]
    public LayerMask GroundLayers;
    public Vector2 RaycastOffset;
    public float MaxGroundDistance;

    private Player assignedPlayer;

    private Rigidbody2D rb;
    private bool jumpPressed;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        assignedPlayer = GetComponent<Player>();
    }

    public void Update()
    {
        if (InputHandler.JumpPressed(assignedPlayer.Index))
        {
            Debug.Log("player " + assignedPlayer.Index + "jumps!");
            jumpPressed = true;
        }
    }

    public void FixedUpdate()
    {
        float horizontalMovement = InputHandler.HorizontalInput(assignedPlayer.Index);
        rb.velocity = new Vector2(horizontalMovement * MoveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        if(jumpPressed)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + RaycastOffset, Vector2.down, MaxGroundDistance, GroundLayers);
            if(hit)
            {
                rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }
        }
        jumpPressed = false;
    }

}
