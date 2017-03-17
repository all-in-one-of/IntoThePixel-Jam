using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject ObjectToThrow;
    public Vector2 Offset;
    public float MaxForce;
    public AnimationCurve ForceFraction;
    public float DeadThreshold = 0.2f;
    public float ThrowThreshold = 1f;

    private Player assignedPlayer;

    private float windupStartTime = -1f;
    private bool throwLocked;

    public void Start()
    {
        assignedPlayer = GetComponent<Player>();
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
        GameObject projectile = GameObject.Instantiate(ObjectToThrow);
        projectile.transform.position = (Vector2)transform.position + Offset;
        projectile.GetComponentInChildren<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        windupStartTime = -1f;
        throwLocked = true;
    }
}
