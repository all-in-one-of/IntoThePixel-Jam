using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableware : MonoBehaviour
{
    public float BreakRelativeVelocityMagnitude;
    public GameObject MainObject;
    public GameObject ParticleSystem;

    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if(collision.relativeVelocity.magnitude >= BreakRelativeVelocityMagnitude)
        {
            MainObject.SetActive(false);
            ParticleSystem.transform.localPosition = transform.localPosition;
            ParticleSystem.SetActive(true); 
        }
    }
}
