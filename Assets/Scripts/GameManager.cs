using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerActivation;

    public List<GameObject> packagePrefabs;

    private bool isAllReady = false;
    private bool playersInit = false;
    
    List<GameObject> isActiveText = new List<GameObject>();
    List<string> activePlayers = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        playerActivation = Instantiate(playerActivation, new Vector3(0, 0, 0), Quaternion.identity);
        //get all player children
        foreach (Transform child in playerActivation.transform)
        {
            //Debug.Log(child.GameObject());
            isActiveText.Add(child.GameObject());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //pre-game -- call while players have not started
        if (!isAllReady)
        {
            checkPlayersReady();
        }

        //initiate game 
        if (!playersInit && isAllReady)
        {
            Debug.Log("run");
            initPlayers();
        }

    }

    //Pre-game -- Player checking
    private void checkPlayersReady()
    {
        //Activate corrisponding player based upon which key is pressed
        if (Input.GetKey(KeyCode.U)&& (activePlayers.IndexOf("p1") == -1))
        {
            //Find the p1 Gameobject in the array
            GameObject p1 = isActiveText.Find(x => x.name == "p1");
            //Set the player to active and add to active players list
            p1.GetComponent<TMP_Text>().text = "1";
            activePlayers.Add("p1");
        }
        
        //Just copied from the p1 logic
        if (Input.GetKey(KeyCode.I) && (activePlayers.IndexOf("p2") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "p2");
            p1.GetComponent<TMP_Text>().text = "1";
            activePlayers.Add("p2");
        }
        
        if (Input.GetKey(KeyCode.O)&& (activePlayers.IndexOf("p3") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "p3");
            p1.GetComponent<TMP_Text>().text = "1";
            activePlayers.Add("p3");
        }
        
        if (Input.GetKey(KeyCode.P)&& (activePlayers.IndexOf("p4") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "p4");
            p1.GetComponent<TMP_Text>().text = "1";
            activePlayers.Add("p4");
        }
        
        //Active players display -- Mostly Debug
        GameObject active = isActiveText.Find(x => x.name == "active");
        active.GetComponent<TMP_Text>().text = "";
        foreach (string item in activePlayers)
        {
            active.GetComponent<TMP_Text>().text += item;
        }

        //When all players are ready, hit k to continue
        if (Input.GetKey(KeyCode.K) && (activePlayers.Count > 1))
        {
            playerActivation.SetActive(false);
            isAllReady = true;
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

            //Instantiate the Player Package prefab
            Instantiate(currentPlayerPackage, new Vector3(0,0,0), Quaternion.identity);
        }
        
        playersInit = true;
    }
}
