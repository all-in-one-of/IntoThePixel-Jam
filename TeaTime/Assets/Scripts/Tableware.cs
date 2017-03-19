using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tableware : MonoBehaviour
{
    private float preparationTime = 0.2f;
    
    public float ShatterPersistenceTime;
    public float ShatterRadius;
    public float KnockbackForce;
    public float BreakRelativeVelocityMagnitude;
    public GameObject MainObject;
    public GameObject ParticleSystem;

    private int belongsToPlayerIndex;

    public void Init(int belongsToPlayerIndex)
    {
        this.belongsToPlayerIndex = belongsToPlayerIndex;
        StartCoroutine(WaitForChangeLayer());
    }   

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(belongsToPlayerIndex != -1)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && player.Index == belongsToPlayerIndex) return;
        }
        if(collision.relativeVelocity.magnitude >= BreakRelativeVelocityMagnitude)
        {
            SoundManager.Instance.shatterSound();
            MainObject.SetActive(false);
            ParticleSystem.transform.localPosition = transform.localPosition;
            ParticleSystem.SetActive(true);

            Player hitPlayer = collision.gameObject.GetComponent<Player>();
            if (hitPlayer == null)
            {
                Player[] players = FindObjectsOfType<Player>();
                foreach (Player player in players)
                {
                    if (Vector2.Distance(player.transform.position, transform.position) < ShatterRadius)
                    {
                        player.GetComponent<Player>().Knockback(this, collision);
                    }
                }
            }
            else
            {
                hitPlayer.Knockback(this, collision);
            }
        }
    }

    private IEnumerator WaitForChangeLayer()
    {
        yield return new WaitForSeconds(preparationTime);
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        belongsToPlayerIndex = -1;
    }
    
    private IEnumerator WaitForShatter()
    {
        yield return new WaitForSeconds(ShatterPersistenceTime);
        gameObject.SetActive(false);
    }

}
