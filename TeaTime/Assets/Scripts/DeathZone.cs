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
            //Debug.Log("Player " + player.Index + " fell off the stage!");
            GameManager.Instance.PlayerFellOffStage(player.Index);
        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
