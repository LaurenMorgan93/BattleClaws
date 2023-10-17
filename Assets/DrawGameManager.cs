using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class DrawGameManager : MonoBehaviour
{
    public List<GameObject> PlayerPackagePrefabs;
    public List<GameObject> collectablesPrefabs;

    public List<GameObject> UIPanels;

    public List<Sprite> playerSprites;
    
    
    List<string> activePlayers = new List<string>();
    

    private void Awake()
    {
        string incomingPlayers = PlayerPrefs.GetString("RemainingPlayers");
        foreach (string item in incomingPlayers.Split(","))
        {
            activePlayers.Add(item);
            Debug.Log(item);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initPanels();
        initPlayers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initPanels()
    {

        for (int i = 0; i < activePlayers.Count; i++)
        {
            GameObject panel = UIPanels[i];
            var playerSprite = playerSprites.Find(x => x.name == activePlayers[i].ToUpper() + " Icon");

            foreach (Transform child in panel.transform)
            {
                if (child.gameObject.name == "Image")
                {
                    child.gameObject.GetComponent<Image>().sprite = playerSprite;
                }

                else if (child.gameObject.name.Split(" ")[1] == "Score")
                {
                    child.gameObject.tag = activePlayers[i] + " Score";
                }
            }

            panel.tag = activePlayers[i] + " Panel";

        }

        foreach (GameObject panel in UIPanels)
        {
            if (activePlayers.Contains(panel.name.Split(" ")[0]))
            {
                panel.SetActive(true);
            }
        }
    }

    public List<String> getActivePlayers()
    {
        return activePlayers;
    }

    //Start game -- load up selected players
    private void initPlayers()
    {
        //Loop through each player listed in the activePlayers which was defined in checkPlayersReady()
        for (var i = 0; i < activePlayers.Count; i++)
        {
            string players = activePlayers[i];
            //Create the search string based on the player prefab naming syntax
            string packageSearch = players.ToString().ToLower() + " Player";
            //Search for the corresponding prefab
            GameObject currentPlayerPackage = PlayerPackagePrefabs.Find(x => x.name == packageSearch);
            
            GameObject[] prefabAnchor = GameObject.FindGameObjectsWithTag("Anchor");
            GameObject anchor = prefabAnchor[i];
            //Instantiate the Player Package prefab
            if (currentPlayerPackage != null)
            {
                var playerInit = Instantiate(currentPlayerPackage, anchor.transform.position, Quaternion.identity);
            }
        }
        
    }
}
