using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public AudioHandler audioPlayingScript;
    public string destinationScene; // choose the scene to load in the inspector

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
        
        if (Input.anyKey)
        {
            LoadDestinationSceneWithDelay();
        }
    }

    private string currentSceneName;

 public void LoadDestinationSceneWithDelay()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
     audioPlayingScript.PlaySoundEffect("Start"); 
        // Wait for 5 seconds.
        yield return new WaitForSeconds(5.0f);

        // Load the destination scene.
        SceneManager.LoadScene(destinationScene);
    }

}


