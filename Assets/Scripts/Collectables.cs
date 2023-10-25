using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Collectables : MonoBehaviour
{
    public GameObject lastPlayer;
    private int pointValue;
    private Claw_Manager PlayerScript;
    public RoundHandler scoreTrackingScript;

    private int pointsStore;
    private Material originalMaterial;
    public Material shineMaterial;

    private float cooldown = 0;

    public void Start()
    {
        originalMaterial = this.GetComponent<Renderer>().material;
        assignPointvalue();
    }

    private void assignPointvalue()
    {
        //Debug.Log("Assign a random value to the collectable object");
        pointValue += Random.Range(50, 250);
        pointsStore = pointValue;
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

  public void activateSuperCharge()
  {
      pointValue *= 2;
      cooldown = 15;
  }

  private void Update()
  {
      if (cooldown >= 0)
      {
          cooldown -= Time.deltaTime;
      }
      else
      {
          pointValue = pointsStore;
      }

      if (cooldown % 2 <= 1 && cooldown > 0)
      {
          this.GetComponent<Renderer>().material = shineMaterial;
      }
      else
      {
          this.GetComponent<Renderer>().material = originalMaterial;
      }
  }
}