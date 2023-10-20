using UnityEngine;

public class RandomEffect : MonoBehaviour
{
    [SerializeField] private int speedBuffAmount;
    [SerializeField] private int speedDebuffAmount;
    [SerializeField] private int frozenSpeed;
    public Claw_Manager PlayerManagerScript;
    public GameObject lastPlayerGrab;

    private readonly string[] randomEffects =
        { "TimeLoss", "SpeedBoost", "SlowPlayers", "FreezePlayers", "DoublePoints", "ShuffleZones" };

    private bool isSpeedBuffed;
    private int pointsValue;

    private TimerScript timerHandlerScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WinZone"))
        {
            if (lastPlayerGrab == null) {return;}
            //Debug.Log("Assign " + pointsValue + " to " + lastPlayerToGrab.name);
            assignRandomEffect();
            //PlayerManagerScript.awardScore(pointsValue);
            gameObject.SetActive(false);
        }
    }
    
    public void assignRandomEffect()
    {
        var randomEffect = randomEffects[Random.Range(0, randomEffects.Length)];
        // Apply the selected random effect
        switch (randomEffect)
        {
            case "TimeLoss":
                // This effect removes 30 seconds from the In-Game Timer
                timerHandlerScript = FindObjectOfType<TimerScript>();
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

                SetOtherPlayersSpeed(0);
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
                var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                gameManager.initDropZones(true);
                Destroy(gameObject);
                break;
        }
    }

    private void SetOtherPlayersSpeed(int speed)
    {
        // List of player tags
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var playerParent in players)
        {
            var playerScript = playerParent.GetComponentInChildren<Claw_Manager>();
            if (playerScript.gameObject != lastPlayerGrab) playerScript.setSpeed(speed);
        }
    }
}