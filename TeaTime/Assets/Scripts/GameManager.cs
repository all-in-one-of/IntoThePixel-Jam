using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public float StartNextRoundTime;
    public float GameOverTime;

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

    public GameObject Game;
    public GameObject Menu;
    public GUI GUI;

    public bool MovementEnabled { get; private set; }

    private Player[] players;

    private Dictionary<int, int> playerScore = new Dictionary<int, int>();
    private bool switchPosition;
    private bool gameOver;
    private int currentRound;

    private static int numberOfRounds = 3;
    private bool gameRunning = false;

    public void Start()
    {
        Menu.SetActive(true);
        Game.SetActive(false);

        players = FindObjectsOfType<Player>();
        playerScore.Add(1, 0);
        playerScore.Add(2, 0);
        //StartRound();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (!gameRunning && Input.GetKey(KeyCode.Return))
            StartGame();
    }

    private void StartGame()
    {
        gameOver = false;
        currentRound = 0;
        gameRunning = true;
        Menu.SetActive(false);
        Game.SetActive(true);
        StartRound(true);
        UpdatePlayerScore();
    }

    private void StartRound(bool firstRound)
    {
        currentRound++;
        MovementEnabled = false;
        TablewareCreator.instance.Reset();
        foreach (Player player in players)
        {
            player.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
            switch (player.Index)
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
        if (firstRound)
        {
            StartCoroutine(GUI.ShowRoundCountdown(1, false, () => MovementEnabled = true));
        }
        else
        {
            StartCoroutine(GUI.ShowRoundCountdown(currentRound, true, () => MovementEnabled = true));
        }
    }

    private void CountDown()
    {
        //CountDownText.gameObject.SetActive(true);
        //MovementEnabled = false;
        //for(int i = CountDownSteps; i >= 0; i--)
        //{
        //    CountDownText.text = i.ToString();
        //    CountDownText.transform.DOScale(2f, CountDownStepDuration / 2f).OnComplete(() => CountDownText.transform.DOScale(1f, CountDownStepDuration / 2f));
        //    yield return new WaitForSeconds(CountDownStepDuration);
        //}
        //CountDownText.gameObject.SetActive(false);
        //MovementEnabled = true;
    }

    public void PlayerFellOffStage(int playerIndex)
    {
        if (gameOver) return;

        gameOver = true;
        int winnerIndex = playerIndex == 1 ? 2 : 1;
        playerScore[winnerIndex]++;
        UpdatePlayerScore();     
        if(winnerIndex == 1)
        {
            ScoreP1.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => ScoreP1.transform.DOScale(1f, StartNextRoundTime / 4f));
        }
        else
        {
            ScoreP2.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => ScoreP2.transform.DOScale(1f, StartNextRoundTime / 4f));
        }
        if (playerScore[winnerIndex] > Mathf.FloorToInt(numberOfRounds / 2f))
        {
            StartCoroutine(GameOver("Player " + winnerIndex + " won!"));
        }
        else
        {
            StartCoroutine(StartNextRound("Player " + playerIndex + " out!"));
        }
    }

    private void UpdatePlayerScore()
    {
        ScoreP1.text = playerScore[1].ToString();
        ScoreP2.text = playerScore[2].ToString();
    }

    private IEnumerator StartNextRound(string message)
    {
        FinishOverlay.gameObject.SetActive(true);
        FinishOverlay.color = new Color(0, 0, 0, 0);
        FinishOverlay.DOColor(new Color(0, 0, 0, 0.8f), StartNextRoundTime / 2f);
        FinishText.text = message;
        FinishText.transform.DOScale(2f, StartNextRoundTime / 4f).OnComplete(() => FinishText.transform.DOScale(1f, StartNextRoundTime / 4f));
        yield return new WaitForSeconds(StartNextRoundTime);
        StartRound(false);
        FinishOverlay.DOColor(new Color(0, 0, 0, 0.0f), StartNextRoundTime / 4f);
        FinishOverlay.gameObject.SetActive(false);
        gameOver = false;
    }

    private IEnumerator GameOver(string message)
    {
        UpdatePlayerScore();
        yield return new WaitForSeconds(GameOverTime);
        for(int i = 0; i < playerScore.Count; i++)
        {
            playerScore[i] = 0;
        }
        Game.SetActive(false);
        Menu.SetActive(true);
        gameRunning = false;
    }

}
