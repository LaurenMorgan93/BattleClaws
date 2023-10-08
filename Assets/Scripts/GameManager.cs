using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public List<GameObject> PlayerPackagePrefabs;
    public List<GameObject> dropZonePrefabs;
    public List<GameObject> collectablesPrefabs;
    public List<GameObject> powerUpsPrefabs;
    
    
    List<string> activePlayers = new List<string>();

    private GameObject[] anchors;

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
        anchors = GameObject.FindGameObjectsWithTag("Drop Anchors");
        
        
        initPlayers();
        initDropZones(false);
        initCollectables(80);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<String> getActivePlayers()
    {
        return activePlayers;
    }

    //Start game -- load up selected players
    private void initPlayers()
    {
        //Loop through each player listed in the activePlayers which was defined in checkPlayersReady()
        foreach (string players in activePlayers)
        {
            //Create the search string based on the player prefab naming syntax
            string packageSearch = players.ToString().ToLower() + " Player";
            //Search for the corresponding prefab
            GameObject currentPlayerPackage = PlayerPackagePrefabs.Find(x => x.name == packageSearch);
            
            GameObject prefabAnchor = GameObject.FindGameObjectWithTag(players.ToString() + "Anchor");
            //Instantiate the Player Package prefab
            if (currentPlayerPackage != null)
            {
                Instantiate(currentPlayerPackage, prefabAnchor.transform.position, Quaternion.identity);
            }
        }
        
    }

    //Initiate Dropzones
    public void initDropZones(bool isReset)
    {
        //Destroy any dropzones already in the scene - this is useful for the drop swap powerup
        if (isReset)
        {
            GameObject[] activeDropzones = GameObject.FindGameObjectsWithTag("DropZone");
            if (activeDropzones.Length > 0)
            {
                foreach (var activeDropzone in activeDropzones)
                {
                    Destroy(activeDropzone);
                }
            }
        }

        List<int> usedDropZones = new List<int>();
        //Randomise dropzone placement
        foreach (GameObject anchor in anchors)
        {
            int currentRandomDropzone = Random.Range(0, dropZonePrefabs.Count);
            
            while(usedDropZones.Contains(currentRandomDropzone)){
                currentRandomDropzone = Random.Range(0, dropZonePrefabs.Count());
                
            }

            usedDropZones.Add(currentRandomDropzone);
            Instantiate(dropZonePrefabs[currentRandomDropzone], anchor.transform.position, Quaternion.identity);
        }
    }

    private void initCollectables(int amount)
    {
        int specialAmount = (int) Mathf.Round(amount * 0.03f);
        int standardAmount = amount - specialAmount;

        for (int count = 0; count <= standardAmount; count++)
        {
            spawnStandardCollectable(Random.Range(50,250));
        }
        
        for (int count = 0; count <= specialAmount; count++)
        {
            spawnSpecialCollectable();
        }

    }

    private void spawnStandardCollectable(int value)
    {
        var chooseRandom = Random.Range(0, collectablesPrefabs.Count);
        Vector3 spawnPos = this.transform.position + new Vector3(Random.Range(-80, 80), 0, Random.Range(-80, 80));
        Instantiate(collectablesPrefabs[chooseRandom], spawnPos, Quaternion.identity);
    }

    private void spawnSpecialCollectable()
    {
        Vector3 spawnPos = this.transform.position + new Vector3(Random.Range(-80, 80), 0, Random.Range(-80, 80));
        Instantiate(powerUpsPrefabs[0], spawnPos, Quaternion.identity);
    }
}
