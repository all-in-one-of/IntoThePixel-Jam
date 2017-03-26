using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    public float StartNextRoundTime;
    public Text FinishText;
    public Image FinishOverlay;
    
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
    private bool gameOver;

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

    public void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private IEnumerator CountDown()
    {
        CountDownText.gameObject.SetActive(true);
        MovementEnabled = false;
        for(int i = CountDownSteps; i >= 0; i--)
        {
            CountDownText.text = i.ToString();
            CountDownText.transform.DOScale(2f, CountDownStepDuration / 2f).OnComplete(() => CountDownText.transform.DOScale(1f, CountDownStepDuration / 2f));
            yield return new WaitForSeconds(CountDownStepDuration);
        }
        CountDownText.gameObject.SetActive(false);
        MovementEnabled = true;
    }

    public void PlayerFellOffStage(int playerIndex)
    {
        if (gameOver) return;
        gameOver = true;
        int winnerIndex = playerIndex == 1 ? 2 : 1;
        playerScore[winnerIndex]++;
        ScoreP1.text = playerScore[1].ToString();
        ScoreP2.text = playerScore[2].ToString();
        if(winnerIndex == 1)
        {
            ScoreP1.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => ScoreP1.transform.DOScale(1f, StartNextRoundTime / 4f));
        }
        else
        {
            ScoreP2.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => ScoreP2.transform.DOScale(1f, StartNextRoundTime / 4f));
        }
        StartCoroutine(StartNextRound("Player " + playerIndex + " out!"));
    }

    private IEnumerator StartNextRound(string message)
    {
        FinishOverlay.gameObject.SetActive(true);
        FinishOverlay.color = new Color(0, 0, 0, 0);
        FinishOverlay.DOColor(new Color(0, 0, 0, 0.8f), StartNextRoundTime / 2f);
        FinishText.text = message;
        FinishText.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => FinishText.transform.DOScale(1f, StartNextRoundTime / 4f));
        yield return new WaitForSeconds(StartNextRoundTime);
        SceneManager.LoadScene(0);
        FinishOverlay.DOColor(new Color(0, 0, 0, 0.0f), StartNextRoundTime / 4f);
        FinishOverlay.gameObject.SetActive(false);
        gameOver = false;
    }
}
