using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundHandler : MonoBehaviour
{
    public TimerScript timerHandlerScript;

    // Create a dictionary to store points for each player tag.
    private Dictionary<string, int> playerPoints = new Dictionary<string, int>();
    public bool hasRoundEnded;
    public GameObject EliminationScreen;
    public GameObject DrawIdentifierScreen;
    public TextMeshProUGUI EliminateTextBox;
    private int totalRounds;
    private int currentRound = 0;
    public TMP_Text currentRoundText;
    public GameManager gameManager;
    

    public void Start()
    {
        EliminationScreen.SetActive(false);
        DrawIdentifierScreen.SetActive(false);

        updateRoundValues();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K) && hasRoundEnded)
        {
            SceneManager.LoadScene("Gameplay_L");
        }
    }

    private void updateRoundValues()
    {
        if (PlayerPrefs.GetInt("TotalRounds") != 0)
        {
            totalRounds = PlayerPrefs.GetInt("TotalRounds");
        }
        else
        {
            var numberOfPlayers = gameManager.getActivePlayers().Count;
            totalRounds = numberOfPlayers - 1;
        }

        if (PlayerPrefs.GetInt("CurrentRound") != 0)
        {
            currentRound = PlayerPrefs.GetInt("CurrentRound") + 1;
        }
        else
        {
            currentRound++;
        }

        if (currentRound > totalRounds)
        {
            //Go to end game logic
            Debug.Log("End Game Here");
        }

        Debug.Log("Round Stats:");
        Debug.Log("Current round:" + currentRound + "/" + (totalRounds));
        currentRoundText.text = "Round " + currentRound.ToString();
    }

    public void getFinalScores()
    {
        List<string> activePlayers = PlayerPrefs.GetString("Players").Split(',').ToList();


        foreach (string player in activePlayers)
        {
            GameObject currentPlayer = GameObject.FindGameObjectWithTag(player + " Player");
            int score = currentPlayer.GetComponent<Claw_Manager>().getPoints();
            playerPoints[player] = score;
        }
    }

    /*// Method to add points for a specific player tag.
    public void AddPointsForPlayer(string playerTag, int points)
    {
        if (playerPoints.ContainsKey(playerTag))
        {
            playerPoints[playerTag] += points;
            Debug.Log(playerTag + " scored " + points + " points. Total points: " + playerPoints[playerTag]);
        }
        else
        {
            // Player tag doesn't exist, add a new entry for the player.
            playerPoints[playerTag] = points;
            Debug.Log(playerTag + " scored " + points + " points. Total points: " + playerPoints[playerTag]);
        }
    }*/

    // Method to get points for a specific player tag.
    public int GetPointsForPlayer(string playerTag)
    {
        if (playerPoints.ContainsKey(playerTag))
        {
            return playerPoints[playerTag];
        }
        else
        {
            return 0;
        }
    }


    public string CompareScores()
    {
        getFinalScores();
        // Initialize variables to keep track of the lowest score and the respective player.
        int lowestScore = int.MaxValue; // Initialize with a value higher than the possible scores.
        string playerWithLowestScore = "";

        // Create a list to store players with identical lowest scores.
        List<string> playersWithIdenticalLowestScores = new List<string>();

        // Iterate through all players' tags to find the one with the lowest score.
        foreach (string playerTag in playerPoints.Keys)
        {
            int playerScore = GetPointsForPlayer(playerTag);

            if (playerScore < lowestScore)
            {
                lowestScore = playerScore;
                playerWithLowestScore = playerTag;

                // Reset the list of players with identical lowest scores.
                playersWithIdenticalLowestScores.Clear();
                playersWithIdenticalLowestScores.Add(playerTag);
            }
            else if (playerScore == lowestScore)
            {
                // Add the player to the list of players with identical lowest scores.
                playersWithIdenticalLowestScores.Add(playerTag);
            }
        }

        // Check if there are players with identical lowest scores.
        if (playersWithIdenticalLowestScores.Count > 1)
        {
            // Handle the case when there are players with identical lowest scores.
            DrawIdentifierScreen.SetActive(true);

            return "Tie among players: " + string.Join(", ", playersWithIdenticalLowestScores);
        }
        else
        {
            // Display text for the player with the lowest score.
            EliminateTextBox.text = (playerWithLowestScore + " Is The Lowest Scorer this round! Total Points: " +
                                     GetPointsForPlayer(playerWithLowestScore));
            EliminationScreen.SetActive(true);
            var currentPlayers = gameManager.getActivePlayers();
            currentPlayers.Remove(playerWithLowestScore);
            PlayerPrefs.SetString("RemainingPlayers",  string.Join( ",", currentPlayers));
            Debug.Log(string.Join( ",", currentPlayers));

            // Return the player tag with the lowest score.
            return playerWithLowestScore;
        }
    }

    public void endRound()
    {
        if (!hasRoundEnded)
        {
            CompareScores();
            hasRoundEnded = true;
            pushRoundData();

            
        }
    }

    private void pushRoundData()
    {
        Debug.Log("Pushing Info!");
        PlayerPrefs.SetInt("TotalRounds", totalRounds);
        PlayerPrefs.SetInt("CurrentRound", currentRound);
    }

    public void loadBonusRound()
    {
        SceneManager.LoadScene("tieBreaker");
    }
}