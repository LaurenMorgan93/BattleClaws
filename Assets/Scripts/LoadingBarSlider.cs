using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarSlider : MonoBehaviour
{
    public Slider slider;          // Reference to the UI Slider.
    public float timeToFill = 10.0f;  // Time in seconds to fill the slider.
    private float fillRate;         // The rate at which the slider fills.
    private float currentTime = 0.0f; // Tracks the current time during filling.
    private bool isFilling = false;

    public string location = "Gameplay_L";// Flag to control filling.

    // Start is called before the first frame update.
    private void Start()
    {
        fillRate = 1.0f / timeToFill; // Calculate the fill rate.
        slider.value = 0.0f;          // Set the slider's initial value to 0.
        StartFilling();              //begin filling when the scene begins
    }

    // Update is called once per frame.
    private void Update()
    {
        if (isFilling)
        {
            currentTime += Time.deltaTime; // Increment the current time.

            if (currentTime >= timeToFill)
            {
                FinishLoading();  // start the gameplay scene
            }
            else
            {
                slider.value = currentTime * fillRate; // Update the slider's value based on time.
            }
        }
    }

    // Start filling the slider.
    public void StartFilling()
    {
        isFilling = true;
    }

    // Finish loading and transition to the next round or scene.
    private void FinishLoading()
    {
        slider.value = 1.0f; // Set the slider to full.
        isFilling = false;   // Stop filling.

        // Load the next round or scene (adjust "NextRound" to your scene name).
        SceneManager.LoadScene(location);
    }
}
