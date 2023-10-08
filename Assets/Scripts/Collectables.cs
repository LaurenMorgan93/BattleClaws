using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectables : MonoBehaviour
{
    private GameObject lastPlayer;
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
        string colliderTag = other.GetComponent<Collider>().tag;

    switch (colliderTag)
    {
        case "p1 Player":
        case "p2 Player":
        case "p3 Player":
        case "p4 Player":
            Debug.Log("Picked up by " + colliderTag);
            lastPlayer = other.gameObject;
            PlayerScript = other.gameObject.GetComponent<Claw_Manager>();
            break;

        case "WinZone":
            if (lastPlayer == null)
            {
                break;
            }

            lastPlayer.GetComponent<Claw_Manager>().addPoints(pointValue);
            Debug.Log(lastPlayer.name + " gained " +pointValue + " points!");
            Destroy(gameObject);
            break;
        }

    }

}