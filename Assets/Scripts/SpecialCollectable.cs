using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCollectable : MonoBehaviour
{
    public GameObject holdingPlayer;
    private bool isHeld = false;

    public RoundHandler roundhandler;

    private void Update()
    {
        if (isHeld && !roundhandler.hasRoundEnded)
        {
            holdingPlayer.GetComponent<Claw_Manager>().addPoints(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
    }

    public void setHeld(bool isHeld)
    {
        this.isHeld = isHeld;
    }
}