using UnityEngine;
using UnityEngine.UI;

public class ReadySlider : MonoBehaviour
{
    public Slider slider;
    public float requiredHoldTime = 3.0f;
    private float fillRate;
    private bool isKeyHeld;
    private float keyHeldTime;
    public PlayerHandover playerReadyScript;

    private void Start()
    {
        // add a reference to Jess's player handover script
        playerReadyScript = FindObjectOfType<PlayerHandover>();
        // set the fillrate for the loading bar
        fillRate = 1.0f / requiredHoldTime;
        
    }

    private void Update()
    {

        if (isKeyHeld)
        {
            //when any key is held down, start filling the ready up bar
            slider.value += fillRate * Time.deltaTime;

            if (slider.value >= 1.0f)
            {
                //when the button has been held long enough to ready up, start the game
                StartNextRound();
            }
        }

        if (Input.anyKey)
        {
            OnKeyHeld();
        }
        else
        {
            OnKeyReleased();
        }
    }

    private void OnKeyHeld()
    {
        if(playerReadyScript.activePlayers.Count >=2)
        {
            isKeyHeld = true;
            keyHeldTime += Time.deltaTime;
            if (keyHeldTime >= requiredHoldTime)
            {
                StartNextRound();
            }

        }

    }

    private void OnKeyReleased()
    {
        // switch the flag to false when the button is not being held
        isKeyHeld = false;

        if (slider.value < 1.0f)
        {
            // call reset slider function 
            ResetSlider();
        }

        keyHeldTime = 0.0f;
    }

    private void StartNextRound()
    {
        // set a bool in Jess's script to true, which then calls the function in his script to begin the game
        playerReadyScript.ReadyToStart = true;
    }

    private void ResetSlider()
    {
        // set the slider value to 0
        slider.value = 0;
    }
}
