using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> packagePrefabs;
    
    private bool playersInit = false;
    List<string> activePlayers = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        string incomingPlayers = PlayerPrefs.GetString("Players");
        foreach (string item in incomingPlayers.Split(","))
        {
            activePlayers.Add(item);
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //initiate game 
        if (!playersInit)
        {
            initPlayers();
        }

    }

    //Start game -- load up selected players
    private void initPlayers()
    {
        //Loop through each player listed in the activePlayers which was defined in checkPlayersReady()
        foreach (string players in activePlayers)
        {
            //Create the search string based on the player prefab naming syntax
            string packageSearch = players.ToString() + " Package";
            //Search for the corresponding prefab
            GameObject currentPlayerPackage = packagePrefabs.Find(x => x.name == packageSearch);
            
            GameObject prefabAnchor = GameObject.FindGameObjectWithTag(players.ToString() + "Anchor");
            //Instantiate the Player Package prefab
            Instantiate(currentPlayerPackage, prefabAnchor.transform.position, Quaternion.identity);
        }
        
        playersInit = true;
    }
}
