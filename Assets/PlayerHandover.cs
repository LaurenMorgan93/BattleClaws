using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHandover : MonoBehaviour
{
    public GameObject playerActivation;
    
    List<GameObject> isActiveText = new List<GameObject>();
    List<string> activePlayers = new List<string>();

    
    private bool isAllReady = false;
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
        if (!isAllReady)
        {
            checkPlayersReady();
        }
        
    }
    
    private void checkPlayersReady()
    {
        //Activate corrisponding player based upon which key is pressed
        if (Input.GetKey(KeyCode.U)&& (activePlayers.IndexOf("p1") == -1))
        {
            //Find the p1 Gameobject in the array
            GameObject p1 = isActiveText.Find(x => x.name == "Player 1");
            //Set the player to active and add to active players list
            p1.GetComponentInChildren<Outline>().enabled = true;
            activePlayers.Add("p1");
        }
        
        //Just copied from the p1 logic
        if (Input.GetKey(KeyCode.I) && (activePlayers.IndexOf("p2") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "Player 2");
            p1.GetComponentInChildren<Outline>().enabled = true;
            activePlayers.Add("p2");
        }
        
        if (Input.GetKey(KeyCode.O)&& (activePlayers.IndexOf("p3") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "Player 3");
            p1.GetComponentInChildren<Outline>().enabled = true;
            activePlayers.Add("p3");
        }
        
        if (Input.GetKey(KeyCode.P)&& (activePlayers.IndexOf("p4") == -1))
        {
            GameObject p1 = isActiveText.Find(x => x.name == "Player 4");
            p1.GetComponentInChildren<Outline>().enabled = true;
            activePlayers.Add("p4");
        }
        
        //Active players display -- Mostly Debug
        GameObject continueButton = isActiveText.Find(x => x.name == "Continue");

        if (activePlayers.Count < 2)
        {
            continueButton.GetComponentInChildren<TMP_Text>().text = "Two or more players required!";
        }
        else
        {
            continueButton.GetComponentInChildren<TMP_Text>().text = "Press B on any controller to continue!";
        }


        //When all players are ready, hit k to continue
        if (Input.GetKey(KeyCode.K) && (activePlayers.Count > 1))
        {
            PlayerPrefs.SetString("Players", string.Join( ",", activePlayers));
            isAllReady = true;
            playerActivation.SetActive(false);
            SceneManager.LoadScene("Mechanics_Gameplay");
        }
    }
}
