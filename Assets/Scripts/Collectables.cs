using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectables : MonoBehaviour
{
   private GameObject lastPlayer;
   private int pointValue;
   private Claw_Manager PlayerScript;

    private void Start()
    {
        assignPointvalue();
    }

  private void assignPointvalue(){

    pointValue += 50;
  }
  
  private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.GetComponent<Collider>().tag;

    switch (colliderTag)
    {
        case "Player One":
        case "Player Two":
        case "Player Three":
        case "Player Four":
            Debug.Log("Picked up by " + colliderTag);
            lastPlayer = other.gameObject;
            PlayerScript = other.gameObject.GetComponent<Claw_Manager>();
            break;

        case "WinZone":
            Debug.Log("Assign " + pointValue + " to " + lastPlayer.name);
            PlayerScript.awardScore(pointValue);
            break;
    }

    }

}

