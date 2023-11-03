using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // add this line whenever you are using UI 
using TMPro; // Add this line whenever you are using Text Mesh Pro 

public class TimerScript : MonoBehaviour
{
    // assign a value in seconds to the timer in the inspector
    [SerializeField] public float timeLeftInRound;
    //a reference to the TextMeshPro text box that will be displayed to the player. Assign me in the inspector!
    public TextMeshProUGUI timerTextObject;
    // bool to check if the timer is activated or not
    public bool countdownActive = true;
    // the string that will be passed to the Timer Text Box 
    private string timeDisplayedString;
    public RoundHandler scoreTrackingScript;



 public void Update() // this happens every frame
 {
    // if the timer is active and should be counting down
    if(countdownActive)
    {
        // subtract from the timeLeftInRound value 
        timeLeftInRound -= Time.deltaTime;
        // Update the timeDisplayedString value in the format Minutes and Seconds
         timeDisplayedString = $"{Mathf.Floor(timeLeftInRound / 60):0}:{timeLeftInRound % 60:00}";
        //Pass this string to the actual Text Mesh Pro Text box for the player to see!
         timerTextObject.text = timeDisplayedString;
    }

        // if the timer has reached 0 or below, 
        if(timeLeftInRound <= 0)
        {
            // Stop the timer counting down by changing the bool to false
            countdownActive = false;

            //change the string
            timeDisplayedString = "ROUND OVER";
            //pass the string to the TMP text box for the player to see
            timerTextObject.text = timeDisplayedString;
            scoreTrackingScript.endRound();
             
        }

        if (timeLeftInRound <= 10)
        {
            timerTextObject.color = Color.red;
            if (timeLeftInRound % 1f >= 0.5f)
            {
                timerTextObject.gameObject.SetActive(false);
            }
            else
            {
                timerTextObject.gameObject.SetActive(true);
            }
        }

 }
}
