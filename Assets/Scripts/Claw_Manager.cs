using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Claw_Manager : MonoBehaviour
{
    [SerializeField] public float buffCooldown;
    [SerializeField] int grabCooldown;
    public int clawMoveSpeed;
    public bool doublePointsBuffActive;
    public GameObject parentObject;
    private GameObject playerOneClaw;
    private GameObject CollectableObject;
    private int defaultSpeed = 50;
    private Animator playerOneAnim;
    private bool isSpeedBuffed;    
    private bool objectGrabbed = false;
    private GameObject heldObject;
    public RoundHandler scoreTrackingScript;
    public string playerIdentifier;

    public string dropKey;

    private TMP_Text scoreUI;

    public int points = 0;
    
    // Define the boundaries for player movement
    private float minX = -150f; // Set your minimum X position
    private float maxX = 140f;  // Set your maximum X position
    private float minZ = -60f; // Set your minimum Z position
    private float maxZ = 235f;  // Set your maximum Z position

    // Start is called before the first frame update
    void Start()
    {
        // Find the Object tagged Player 
        playerOneClaw = gameObject;
        scoreUI = GameObject.FindGameObjectWithTag((playerIdentifier + " Score")).GetComponent<TMP_Text>();
        scoreUI.text = "000000";
        
        scoreTrackingScript = GameObject.FindGameObjectWithTag("RoundHandler").GetComponent<RoundHandler>();
    }

    public void moveClaw()
    {
        //This code gets the X and Z input and moves the Player object with it.
        //float Xmovement = Input.GetAxis("Hor_" + pName);
        float Xmovement = Input.GetAxis("Hor_" + playerIdentifier);
        float Ymovement = Input.GetAxis("Vert_" + playerIdentifier);
        Vector3 movement = new Vector3(Xmovement, 0, Ymovement);
        // Clamp the player's position within the boundaries
        parentObject.transform.Translate(movement * clawMoveSpeed * Time.deltaTime);
        Vector3 newPosition = parentObject.transform.position + movement * clawMoveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
        // Update the player's position
        parentObject.transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        moveClaw();
        PlayerGrab();
        if(grabCooldown > 0)
        {
            grabCooldown--;
        }

        if (isSpeedBuffed){
            buffCooldown -= Time.deltaTime;

            if(buffCooldown <= 0){
                isSpeedBuffed = false;
                clawMoveSpeed = defaultSpeed;
            }
        } 
        if(doublePointsBuffActive)
        {
            buffCooldown -= Time.deltaTime;
            if(buffCooldown <= 0)
            {
                doublePointsBuffActive = false;
            }
        }
    }

    private void PlayerGrab()
    {
        // If the player presses dG, run the animation that makes the Claw drop down.
        if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), dropKey)))
        {
            if (!objectGrabbed && grabCooldown == 0)
            {
                playerOneAnim = playerOneClaw.GetComponent<Animator>();
                playerOneAnim.SetTrigger("Grab");
                grabCooldown = 100;
                objectGrabbed = true; // Object is grabbed here.
            }
            else if (objectGrabbed)
            {
                // Drop the object by destroying the FixedJoint on the collectable object.
                FixedJoint[] fixedJoints = GetComponentsInChildren<FixedJoint>();
                foreach (FixedJoint joint in fixedJoints)
                {
                    Destroy(joint);
                }

                if (heldObject && scoreTrackingScript.isDrawRound == true)
                {
                    heldObject.GetComponent<SpecialCollectable>().setHeld(false);
                }

                objectGrabbed = false;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
        //  Debug.Log("Trigger entered with: " + other.gameObject.name);
            // Check if a FixedJoint already exists on the current GameObject.
            FixedJoint existingJoint = GetComponent<FixedJoint>();

            if (existingJoint == null)
            {
                // If no FixedJoint exists, add one.
                FixedJoint grabbingJoint = gameObject.AddComponent<FixedJoint>();
                grabbingJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                grabbingJoint.anchor = new Vector3(0, -2, 0);
                grabCooldown = 100;
                objectGrabbed = true;

                heldObject = other.gameObject;
            }
        }

        if(other.gameObject.tag == "MysteryBox")
        {
            //  Debug.Log("Trigger entered with: " + other.gameObject.name);
            // Check if a FixedJoint already exists on the current GameObject.
            FixedJoint existingJoint = GetComponent<FixedJoint>();

            if (existingJoint == null)
            {
                // If no FixedJoint exists, add one.
                FixedJoint grabbingJoint = gameObject.AddComponent<FixedJoint>();
                grabbingJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                grabCooldown = 100;
                objectGrabbed = true;
            }
        }


        if(other.gameObject.tag.EndsWith("Player"))
             
            {
                Debug.Log("Collided with another claw!");
                other.gameObject.GetComponent<FixedJoint>();
                // destroy the fixed joint on the other claw's object 
                Destroy(other.gameObject.GetComponent<FixedJoint>());
                // destroy the fixed joint on this claw's object
                Destroy(gameObject.GetComponent<FixedJoint>());
            }

         if(other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject.GetComponent<FixedJoint>());
            }
        }
        public void setSpeed(int value)
        {
            defaultSpeed = clawMoveSpeed;
            clawMoveSpeed = value;
            Debug.Log(clawMoveSpeed);
            buffCooldown = 10;
            isSpeedBuffed = true;
        }

        public void addPoints(int value)
        {
            Debug.Log("Added " + value + "Points!");
            points += value;
            scoreUI.text = points.ToString().PadLeft(6, '0');
        }

        public int getPoints()
        {
            return points;
        }

}