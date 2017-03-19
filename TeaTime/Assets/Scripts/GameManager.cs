using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;

    [Header("Score")]
    public Text ScoreP1;
    public Text ScoreP2;

    [Header("Countdown")]
    public Text CountDownText;
    public float CountDownStepDuration;
    public int CountDownSteps;

    [Header("SpawnPos")]
    public Vector2 SpawnPoint1;
    public Vector2 SpawnPoint2;

    public bool MovementEnabled { get; private set; }

    private Dictionary<int, int> playerScore = new Dictionary<int, int>();
    private bool switchPosition;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.sceneLoaded += (scene, mode) => OnLoad();
        }
        playerScore.Add(1, 0);
        playerScore.Add(2, 0);
    }

    public void OnLoad()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach(Player player in players)
        {
            switch(player.Index)
            {
                case 1:
                    player.transform.position = switchPosition ? SpawnPoint2 : SpawnPoint1;
                    break;
                case 2:
                    player.transform.position = switchPosition ? SpawnPoint1 : SpawnPoint2;
                    break;
            }
        }
        switchPosition = !switchPosition;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        CountDownText.gameObject.SetActive(true);
        MovementEnabled = false;
        for(int i = CountDownSteps; i >= 0; i--)
        {
            CountDownText.text = i.ToString();
            yield return new WaitForSeconds(CountDownStepDuration);
        }
        CountDownText.gameObject.SetActive(false);
        MovementEnabled = true;
    }

    public void PlayerFellOffStage(int playerIndex)
    {
        playerScore[playerIndex == 1 ? 2 : 1]++;
        ScoreP1.text = playerScore[1].ToString();
        ScoreP2.text = playerScore[2].ToString();
    }
}
