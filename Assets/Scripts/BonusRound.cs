using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRound : MonoBehaviour
{

    private Dictionary<string, int> BonusRoundPlayers;

        public void Start()
        {
            // Pull the list of players who drew in the last round from PlayerPrefs (Set in the Round Handler Script)
            string drawingPlayersString = PlayerPrefs.GetString("DrawingPlayers");
            List<string> playerTags = new List<string>(drawingPlayersString.Split(','));

            // Set up the dictionary 
            BonusRoundPlayers = new Dictionary<string, int>();

            foreach (string playerTag in playerTags)
            {
                // Set everyones score to zero 
                BonusRoundPlayers[playerTag] = 0;
            }
        }

     public void addTimeHeldToPlayerScore(string PlayerTag, int secondsHeld)
        {
            if (BonusRoundPlayers.ContainsKey(PlayerTag))
            {
                BonusRoundPlayers[PlayerTag] += secondsHeld;
                Debug.Log("Points added to " + PlayerTag + ": " + secondsHeld);
            }
            else
            {
                Debug.Log("Player not found: " + PlayerTag);
            }
        }
}





    



