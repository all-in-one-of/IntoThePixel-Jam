using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject ObjectToThrow;
    public float Offset;
    public float MaxForce;
    public AnimationCurve ForceFraction;
    public float DeadThreshold = 0.2f;
    public float ThrowThreshold = 1f;

    private Player assignedPlayer;

    private float windupStartTime = -1f;
    private bool throwLocked;
    private Rigidbody2D rb;

    public void Start()
    {
        assignedPlayer = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Vector2 input = InputHandler.AimingDirection(assignedPlayer.Index);
        if(throwLocked)
        {
            if(input.magnitude <= DeadThreshold)
            {
                throwLocked = false;
            }
        }
        else if(windupStartTime < 0)
        {
            if(input.magnitude >= DeadThreshold)
            {
                windupStartTime = Time.time;
            }
        }
        else if(input.magnitude >= ThrowThreshold)
        {
            float force = ForceFraction.Evaluate(Time.time - windupStartTime) * MaxForce;
            Throw(force, input.normalized);
        }
    }

    private void Throw(float force, Vector2 direction)
    {
        Tableware projectile = GameObject.Instantiate(ObjectToThrow).GetComponentInChildren<Tableware>();

        projectile.transform.position = (Vector2)transform.position + direction.normalized * Offset;
        projectile.transform.eulerAngles = new Vector3(0, 0, direction.GetAngle());

        Rigidbody2D projectileRb = projectile.GetComponentInChildren<Rigidbody2D>();
        projectileRb.velocity = rb.velocity;
        projectileRb.AddForce(direction * force, ForceMode2D.Impulse);

        projectile.Init(assignedPlayer.Index);

        windupStartTime = -1f;
        throwLocked = true;
    }
}
