using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public Vector2 SpawnPoint1;
    public Vector2 SpawnPoint2;

    public GameObject Game;
    public GameObject Menu;
    public GUI GUI;

    public bool MovementEnabled { get; private set; }

    private Player[] players;

    private Dictionary<int, int> playerScore = new Dictionary<int, int>();
    private bool switchPosition;
    private bool transitionInProgess;
    private bool gameRunning;
    private int currentRound;

    private static int numberOfRounds = 3;

    public void Start()
    {
        Menu.SetActive(true);
        Game.SetActive(false);

        players = FindObjectsOfType<Player>();
        playerScore.Add(1, 0);
        playerScore.Add(2, 0);
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
        currentRound = 0;
        gameRunning = true;

        Menu.SetActive(false);
        Game.SetActive(true);

        StartRound(true);

        GUI.UpdatePlayerScore(playerScore);
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

    public void PlayerFellOffStage(int playerIndex)
    {
        if (transitionInProgess) return;
        transitionInProgess = true;

        int winnerIndex = playerIndex == 1 ? 2 : 1;
        playerScore[winnerIndex]++;
        GUI.UpdatePlayerScore(playerScore);
        GUI.IndicateScoreChange(winnerIndex);

        if (playerScore[winnerIndex] > Mathf.FloorToInt(numberOfRounds / 2f))
        {
            //TODO: Add beautiful transition curtain here
            StartCoroutine(GUI.ShowResult(2f, "Player " + winnerIndex + " won!", () =>
            {
                ResetScore();
                Game.SetActive(false);
                Menu.SetActive(true);

                gameRunning = false;
                transitionInProgess = false;
            }));
        }
        else
        {
            //TODO: Add beautiful transition curtain here
            StartCoroutine(GUI.ShowResult(1f, "Player " + playerIndex + " out!", () =>
            {
                StartRound(false);
                transitionInProgess = false;
            }));
        }
    }

    private void ResetScore()
    {
        for (int i = 0; i < playerScore.Count; i++)
        {
            playerScore[i] = 0;
        }
    }
}
