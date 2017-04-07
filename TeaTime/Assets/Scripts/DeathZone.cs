using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            GameManager.Instance.PlayerFellOffStage(player.Index);
        }
        else
        {
            Tableware tableware = collision.GetComponent<Tableware>();
            tableware.OnDestroyed(0f);
        }
    }
}
