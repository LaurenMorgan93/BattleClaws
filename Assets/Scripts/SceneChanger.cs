using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public AudioHandler audioPlayingScript;
    public string destinationScene; // Choose the scene to load in the inspector
    private bool SceneChanging = false;
    private Animator thisAnim;

    // Declare currentSceneName
    private string currentSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // Get the name of the current scene
        currentSceneName = SceneManager.GetActiveScene().name;
        audioPlayingScript = FindObjectOfType<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(!SceneChanging)
        {
            if (Input.anyKey)
            {
                
                LoadDestinationSceneWithDelay();
                SceneChanging = true;

            }

        }

    }

    public void LoadDestinationSceneWithDelay()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Assuming you want to play the "Start" audio clip.
        audioPlayingScript.PlaySoundEffect("Start");

        if(currentSceneName == "Splash")
        {
            thisAnim = gameObject.GetComponent<Animator>();
            thisAnim.SetTrigger("Start");
        }

        // Wait for 1 seconds.
        yield return new WaitForSeconds(1.0f);

        // Load the destination scene.
        SceneManager.LoadScene(destinationScene);
    }
}
