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

    [Header("Knockback")]
    public float KnockbackCooldown;
    public float KnockbackMovementFraction;
    public AnimationCurve KnockbackCompensation;

    private Player assignedPlayer;

    private Rigidbody2D rb;
    private bool jumpPressed;
    private bool knockbackCooldownActive;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        assignedPlayer = GetComponent<Player>();
    }

    public void Update()
    {
        if (knockbackCooldownActive) return;
        if (InputHandler.JumpPressed(assignedPlayer.Index))
        {
            jumpPressed = true;
        }
    }

    public void FixedUpdate()
    {
        float horizontalMovement = InputHandler.HorizontalInput(assignedPlayer.Index);
        if (!knockbackCooldownActive)
        {
            rb.velocity = new Vector2(horizontalMovement * MoveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.AddForce(Vector2.right * horizontalMovement * MoveSpeed * KnockbackMovementFraction * Time.fixedDeltaTime);
        }
        if(jumpPressed && rb.velocity.y <= 0 && isGrounded())
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
        jumpPressed = false;
    }

    public void Knockback(Tableware tableware, Collision2D collision)
    {
        Vector2 contactNormal = tableware.transform.position - transform.position;
        float knockbackCompensation = KnockbackCompensation.Evaluate(Mathf.Abs(Vector2.Dot(Vector2.right, contactNormal)));
        rb.AddForce(contactNormal * tableware.KnockbackForce * knockbackCompensation, ForceMode2D.Impulse);
        StartCoroutine(WaitForKnockbackCooldown());
    }

    private IEnumerator WaitForKnockbackCooldown()
    {
        knockbackCooldownActive = true;
        yield return new WaitForSeconds(KnockbackCooldown);
        knockbackCooldownActive = false;
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast((Vector2)transform.position + RaycastOffset, Vector2.down, MaxGroundDistance, GroundLayers) ||
            Physics2D.Raycast((Vector2)transform.position + RaycastOffset + Vector2.right * transform.localScale.x / 2f, Vector2.down, MaxGroundDistance, GroundLayers) ||
            Physics2D.Raycast((Vector2)transform.position + RaycastOffset + Vector2.left * transform.localScale.x / 2f, Vector2.down, MaxGroundDistance, GroundLayers);
    }
}
