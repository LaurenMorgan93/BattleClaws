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
    public bool isDrawRound = false;
    private bool roundIsDraw = false;
    public TMP_Text currentRoundText;
    public GameManager gameManager;
    public DrawGameManager DrawGameManager;
    
    public TextMeshProUGUI DrawTextBox;
    public Animator roundScreenAnim;

    public void Start()
    {
        EliminationScreen.SetActive(false);
        DrawIdentifierScreen.SetActive(false);

        updateRoundValues();
    }

    private void Update()
    {
        if (Input.anyKey && hasRoundEnded && !roundIsDraw)
        {
            SceneManager.LoadScene("Gameplay_L");
        }
        else if (Input.anyKey && hasRoundEnded && roundIsDraw)
        {
            SceneManager.LoadScene("Gameplay_Draw");
        }
    }

    private void updateRoundValues()
    {
        if (PlayerPrefs.GetString("isDraw") == "true")
        {
            isDrawRound = true;
            currentRoundText.text = "Bonus Round!";
            return;
        }

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
            Debug.Log("Increment Round! " + currentRound);
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
        Debug.Log("Final Scores Reached!");
        List<string> activePlayers = PlayerPrefs.GetString("RemainingPlayers").Split(',').ToList();
        
        foreach (string player in activePlayers)
        {
            Debug.Log(player);
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
        Debug.Log("Compare Scores Reached!");
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
                
                PlayerPrefs.SetString("DrawingPlayers", string.Join(",", playersWithIdenticalLowestScores));
                PlayerPrefs.SetString("isDraw", "true");
            }
        }

        // Check if there are players with identical lowest scores.
        if (playersWithIdenticalLowestScores.Count > 1)
        {
            Debug.Log("Draw logic Reached!");
            roundIsDraw = true;
            DrawTextBox.text = playersWithIdenticalLowestScores[0] + " VS " + playersWithIdenticalLowestScores[1];
            return "Tie among players: " + string.Join(", ", playersWithIdenticalLowestScores);
        }
        else
        {
            Debug.Log("standard Scores Reached!" + playerWithLowestScore);
            // Display text for the player with the lowest score.
            EliminateTextBox.text = (playerWithLowestScore.ToUpper() + "<br>" + GetPointsForPlayer(playerWithLowestScore).ToString().PadLeft(6, '0'));
            
            if (isDrawRound)
            {
                var currentPlayers = DrawGameManager.getActivePlayers();
                currentPlayers.Remove(playerWithLowestScore);
                PlayerPrefs.SetString("RemainingPlayers",  string.Join( ",", currentPlayers));
                Debug.Log(string.Join( ",", currentPlayers));
            }
            else
            {
                var currentPlayers = gameManager.getActivePlayers();
                currentPlayers.Remove(playerWithLowestScore);
                PlayerPrefs.SetString("RemainingPlayers",  string.Join( ",", currentPlayers));
                Debug.Log(string.Join( ",", currentPlayers));
            }

            // Return the player tag with the lowest score.
            return playerWithLowestScore;
        }
    }

    public void endRound()
    {
        if (!hasRoundEnded)
        {
            CompareScores();
            EliminateLoser();
            

            if (roundIsDraw)
            {
                declareDraw();
            }
            pushRoundData();
            hasRoundEnded = true;

        }
    }
    
    public void declareDraw()
    {
        Debug.Log("draw logic reached");
        DrawIdentifierScreen.SetActive(true);
        roundScreenAnim.SetTrigger("Draw");
    }
    public void EliminateLoser()
    {
        string playerWithLowestScore = CompareScores(); // Get the player with the lowest score.
        // Check if there's only one player with the lowest score and remove them.
        if (playerWithLowestScore != "" && !roundIsDraw)
        {
            EliminationScreen.SetActive(true);
            roundScreenAnim.SetTrigger("Elim");
            List<string> activePlayers = PlayerPrefs.GetString("RemainingPlayers").Split(',').ToList();
            activePlayers.Remove(playerWithLowestScore);
            PlayerPrefs.SetString("RemainingPlayers", string.Join(",", activePlayers)); // Update PlayerPrefs with the modified active players list.
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
        SceneManager.LoadScene("Gameplay_Draw");
    }
}