using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEffect : MonoBehaviour
{
   [SerializeField] private int speedBuffAmount;
   [SerializeField] private int speedDebuffAmount;
   [SerializeField] private int frozenSpeed;
    public Claw_Manager PlayerManagerScript;
    private TimerScript timerHandlerScript;
    private GameObject lastPlayerGrab;
    private int pointsValue;
    private bool isSpeedBuffed;
    private string grabbingPlayerString;
    private string[] randomEffects = { "TimeLoss", "SpeedBoost", "SlowPlayers", "FreezePlayers", "DoublePoints", "ShuffleZones" };


    public void assignRandomEffect()
    {
      string randomEffect = randomEffects[Random.Range(0, randomEffects.Length)];
        // Apply the selected random effect
        switch (randomEffect)
        {
            case "TimeLoss":
                // This effect removes 30 seconds from the In-Game Timer
                 timerHandlerScript = GameObject.FindObjectOfType<TimerScript>();
                 timerHandlerScript.timeLeftInRound -= 30.0f;
                 Debug.Log("EFFECT 1 Lost 30 seconds!");
                 

                break;

            case "SpeedBoost":
                // This effect changed the current move speed  of the player who scored the mystery box for 10 seconds.
                PlayerManagerScript.setSpeed(speedBuffAmount);
                isSpeedBuffed = true;
                Debug.Log("Speed Boost " + lastPlayerGrab.tag + " for 10 seconds");

                break;

             case "SlowPlayers":
              // This effect changes the OTHER players move speed for 10 seconds.
             SetOtherPlayersSpeed(speedDebuffAmount);            
             isSpeedBuffed = true;
             Debug.Log("slowing players for 10 seconds");
            break;

             case "FreezePlayers":
            //  This effect freezes the OTHER players by setting their move speed to 0 for 10 seconds.
             
             freezeOtherPlayerSpeed(frozenSpeed);
             Debug.Log("Freeze Players For 10 Seconds");
             isSpeedBuffed = true;
             break;

            case "DoublePoints":
            // this effects doubles the value of collectables when awarding a score for 10 seconds
            Debug.Log("Double Points for 10 seconds");
             PlayerManagerScript.doublePointsBuffActive = true;
             PlayerManagerScript.buffCooldown = 10;
            Destroy(gameObject);
             break;
            
            case "ShuffleZones":
                GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gameManager.initDropZones(true);
                Destroy(gameObject);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string collidersTag = other.GetComponent<Collider>().tag;

    switch (collidersTag)
    {
        case "WinZone":
            if (lastPlayerGrab == null)
            {
                break;
            }

            //Debug.Log("Assign " + pointsValue + " to " + lastPlayerToGrab.name);
            assignRandomEffect();
            //PlayerManagerScript.awardScore(pointsValue);
            gameObject.SetActive(false);
            break;
        default:
            lastPlayerGrab = other.gameObject;
            grabbingPlayerString = collidersTag;
            PlayerManagerScript = other.gameObject.GetComponent<Claw_Manager>();
            break;
    }

    }

    private void SetOtherPlayersSpeed(float speed)
    {
        // List of player tags
        List<string> playerTags = new List<string> { "Player One", "Player Two"}; //, "Player Three", "Player Four" };

        foreach (string tag in playerTags)
        {
            if (tag != grabbingPlayerString)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject player in players)
                {
                    // Check if the player object has a PlayerManagerScript
                     PlayerManagerScript = player.GetComponent<Claw_Manager>();
                    if (PlayerManagerScript != null)
                    {
                        // Set the speed of other players to the specified value
                        PlayerManagerScript.setSpeed(speedDebuffAmount);
                    }
                }
            }
        }
    }

    private void freezeOtherPlayerSpeed(float speed) // find a better way to do this without so many functions
    {
        // List of player tags
        List<string> playerTags = new List<string> { "Player One", "Player Two"}; //, "Player Three", "Player Four" };

        foreach (string tag in playerTags)
        {
            if (tag != grabbingPlayerString)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject player in players)
                {
                    // Check if the player object has a PlayerManagerScript
                     PlayerManagerScript = player.GetComponent<Claw_Manager>();
                    if (PlayerManagerScript != null)
                    {
                        // Set the speed of other players to 0 temporarily
                        PlayerManagerScript.setSpeed(frozenSpeed);
                    }
                }
            }
        }
    }
}
