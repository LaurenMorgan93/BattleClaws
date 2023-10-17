using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCollectable : MonoBehaviour
{
    private int secondsHeld; 
    private bool SpecialCollectableisHeld;
    private GameObject holdingPlayer;
    private bool isHeld = false;
    public void OnTriggerStay(Collider other)
    {
        isHeld = true;
        holdingPlayer = other.gameObject;
        
    }

    private void Update()
    {
        if (isHeld)
        {
            holdingPlayer.GetComponent<Claw_Manager>().addPoints(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if (transform.position.y < -6)
        {
            isHeld = false;
        }
    }

    public void setHeld(bool isHeld)
    {
        this.isHeld = isHeld;
    }
}