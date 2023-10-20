using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectables : MonoBehaviour
{
    public GameObject lastPlayer;
    private int pointValue;
    private Claw_Manager PlayerScript;
    public RoundHandler scoreTrackingScript;

    public void Start()
    {
        assignPointvalue();
    }

    private void assignPointvalue()
    {
        //Debug.Log("Assign a random value to the collectable object");
        pointValue += Random.Range(50, 250);
    }
    
  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinZone"))
        {
            if (other.gameObject.transform.parent.gameObject.name.Split(" ")[0] == gameObject.name.Split(" ")[0] && lastPlayer != null){
                lastPlayer.GetComponent<Claw_Manager>().addPoints(pointValue);
                Debug.Log(lastPlayer.name + " gained " + pointValue + " points!");
                Destroy(gameObject);
            }
            
        }
    }
}