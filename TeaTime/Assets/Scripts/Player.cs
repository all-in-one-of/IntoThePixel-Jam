using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Index;

    [Header("Throwing")]
    public GameObject ObjectToThrow;
    public float Offset = 0.051f;
    public float MaxForce = 0.05f;
    public AnimationCurve ForceFraction;
    public float DeadThreshold = 0.1f;
    public float ThrowThreshold = 0.9f;
    public GameObject SpecialTablewareIndicator;

    [Header("Movement")]
    public float MoveSpeed = 65;
    public float JumpForce = 0.03f;

    [Header("GroundCheck")]
    public LayerMask GroundLayers;
    public Vector2 RaycastOffset = new Vector2(0, 0.05f);
    public float MaxGroundDistance = 0.01f;

    [Header("Knockback")]
    public float KnockbackCooldown = 0.3f;
    public float KnockbackMovementFraction = 0.01f;
    public AnimationCurve KnockbackCompensation;

    private Rigidbody2D rb;
    private Animator anim;
    private bool jumpPressed;
    private bool knockbackCooldownActive;
    private float windupStartTime = -1f;
    private bool throwLocked;
    private bool grounded;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        if (!GameManager.Instance.MovementEnabled) return;

        if (!knockbackCooldownActive)
        {
            if (InputHandler.JumpPressed(Index))
            {
                jumpPressed = true;
            }
        }

        Vector2 input = InputHandler.AimingDirection(Index);
        if (throwLocked)
        {
            if (input.magnitude <= DeadThreshold)
            {
                throwLocked = false;
            }
        }
        else if (windupStartTime < 0)
        {
            if (input.magnitude >= DeadThreshold)
            {
                windupStartTime = Time.time;
            }
        }
        else if (input.magnitude >= ThrowThreshold)
        {
            float force = ForceFraction.Evaluate(Time.time - windupStartTime) * MaxForce;
            Throw(force, input.normalized);
        }
    }

    public void FixedUpdate()
    {
        CheckGroundContact();
        if (!GameManager.Instance.MovementEnabled) return;

        float horizontalMovement = InputHandler.HorizontalInput(Index);
        anim.SetFloat("horizontalMovement", Mathf.Abs(horizontalMovement));
        float moveSpeed = horizontalMovement * MoveSpeed * Time.fixedDeltaTime;
        if (!knockbackCooldownActive)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.AddForce(Vector2.right * moveSpeed * KnockbackMovementFraction * Time.fixedDeltaTime);
        }

        if (jumpPressed && rb.velocity.y <= 0 && grounded)
        {
            float jumpForce = JumpForce;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        jumpPressed = false;
    }

    public void Knockback(Tableware tableware, Collision2D collision)
    {
        Vector2 contactNormal = (transform.position - tableware.transform.position).normalized;
        float knockbackCompensation = KnockbackCompensation.Evaluate(Mathf.Abs(Vector2.Dot(Vector2.right, contactNormal)));
        rb.AddForce(contactNormal * tableware.KnockbackForce * knockbackCompensation, ForceMode2D.Impulse);
        StartCoroutine(WaitForKnockbackCooldown());
    }

    private void Throw(float force, Vector2 direction)
    {
        anim.SetTrigger("throw");
        GameObject objectToThrow;
        objectToThrow = Instantiate(ObjectToThrow);
        Tableware projectile = objectToThrow.GetComponentInChildren<Tableware>();

        projectile.transform.position = (Vector2)transform.position + direction.normalized * Offset;
        projectile.transform.eulerAngles = new Vector3(0, 0, direction.GetAngle());

        Rigidbody2D projectileRb = projectile.GetComponentInChildren<Rigidbody2D>();
        projectileRb.velocity = rb.velocity;
        projectileRb.AddForce(direction * force, ForceMode2D.Impulse);

        projectile.Init(Index);

        windupStartTime = -1f;
        throwLocked = true;
    }

    private bool CheckGroundContact()
    {
        grounded = Physics2D.Raycast((Vector2)transform.position + RaycastOffset, Vector2.down, MaxGroundDistance, GroundLayers) ||
            Physics2D.Raycast((Vector2)transform.position + RaycastOffset + Vector2.right * transform.localScale.x / 2f, Vector2.down, MaxGroundDistance, GroundLayers) ||
            Physics2D.Raycast((Vector2)transform.position + RaycastOffset + Vector2.left * transform.localScale.x / 2f, Vector2.down, MaxGroundDistance, GroundLayers);
        anim.SetBool("grounded", grounded);
        return grounded;
    }

    private IEnumerator WaitForKnockbackCooldown()
    {
        knockbackCooldownActive = true;
        yield return new WaitForSeconds(KnockbackCooldown);
        knockbackCooldownActive = false;
    }
}
