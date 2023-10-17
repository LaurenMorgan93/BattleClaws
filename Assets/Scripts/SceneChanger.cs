using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string destinationScene; // choose the scene to load in the inspector

    // Start is called before the first frame update
    void Start()
    {
        // Get the name of the current scene
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.anyKey)
        {
            LoadDestinationScene();
        }
    }

    private string currentSceneName;

    public void LoadDestinationScene()
    {
        SceneManager.LoadScene(destinationScene);
    }
}


