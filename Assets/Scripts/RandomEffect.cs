using TMPro;
using UnityEngine;

public class RandomEffect : MonoBehaviour
{
    [SerializeField] private int speedBuffAmount;
    [SerializeField] private int speedDebuffAmount;
    public Claw_Manager PlayerManagerScript;
    public GameObject lastPlayerGrab;
    private GameManager gameManager;

    private string[] randomEffects = { "SpeedBoost", "SlowPlayers", "DoublePoints", "ShuffleZones", "LockZone"/*, "ColourDrain" */};
    //private readonly string[] randomEffects = { "SlowPlayers" };
    private RoundHandler roundHandlerScript;
    private TimerScript timerHandlerScript;


    private void Start()
    {
        roundHandlerScript = FindObjectOfType<RoundHandler>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collidersTag = other.GetComponent<Collider>().tag;

        switch (collidersTag)
        {
            case "WinZone":
                if (lastPlayerGrab == null) break;

                //Debug.Log("Assign " + pointsValue + " to " + lastPlayerToGrab.name);
                assignRandomEffect();
                //PlayerManagerScript.awardScore(pointsValue);
                gameObject.SetActive(false);
                break;
            default:
                lastPlayerGrab = other.gameObject;
                PlayerManagerScript = other.gameObject.GetComponent<Claw_Manager>();
                break;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void assignRandomEffect()
    {
        var randomEffect = randomEffects[Random.Range(0, randomEffects.Length)];
        roundHandlerScript.randomEffectScreen.GetComponentInChildren<TMP_Text>().text = randomEffect + "!";
        roundHandlerScript.randomEffectScreen.SetActive(true);

        // Apply the selected random effect
        switch (randomEffect)
        {
            case "SpeedBoost":
                // This effect changed the current move speed  of the player who scored the mystery box for 10 seconds.
                PlayerManagerScript.setSpeed(speedBuffAmount);
                Debug.Log("Speed Boost " + lastPlayerGrab.tag + " for 10 seconds");

                break;

            case "SlowPlayers":
                // This effect changes the OTHER players move speed for 10 seconds.
                SetOtherPlayersSpeed(speedDebuffAmount);
                Debug.Log("slowing players for 10 seconds");
                break;

            case "DoublePoints":
                // this effects doubles the value of collectables when awarding a score for 10 seconds
                Debug.Log("Double Points for 10 seconds");
                PlayerManagerScript.doublePointsBuffActive = true;
                PlayerManagerScript.buffCooldown = 10;
                Destroy(gameObject);
                break;

            case "ShuffleZones":
                gameManager.initDropZones(true);
                Destroy(gameObject);
                break;

            case "LockZone":
                gameManager.lockRandomDropZone();
                Destroy(gameObject);
                break;
        }
    }

    private void SetOtherPlayersSpeed(float speed)
    {
        var activePlayers = gameManager.ActivePlayerObjects;

        foreach (var player in activePlayers)
        {
            // Check if the player object has a PlayerManagerScript
            var currentPlayerManagerScript = player.GetComponentInChildren<Claw_Manager>();
            if (currentPlayerManagerScript != null && currentPlayerManagerScript != PlayerManagerScript)
                // Set the speed of other players to the specified value
                currentPlayerManagerScript.setSpeed(speedDebuffAmount);
        }
    }
}