using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableware : MonoBehaviour
{
    public float ShatterPersistenceTime;
    public float ShatterRadius;
    public float KnockbackForce;
    public float BreakRelativeVelocityMagnitude;
    public GameObject MainObject;
    public GameObject ParticleSystem;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude >= BreakRelativeVelocityMagnitude)
        {
            MainObject.SetActive(false);
            ParticleSystem.transform.localPosition = transform.localPosition;
            ParticleSystem.SetActive(true);

            Player[] players = FindObjectsOfType<Player>();
            foreach(Player player in players)
            {
                if(Vector2.Distance(player.transform.position, transform.position) < ShatterRadius)
                {
                    player.GetComponent<CharacterMovement>().Knockback(this, collision);
                }
            }
        }
    }

    private IEnumerator WaitForShatter()
    {
        yield return new WaitForSeconds(ShatterPersistenceTime);
        gameObject.SetActive(false);
    }

}
