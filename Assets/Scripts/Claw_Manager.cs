using System.Collections;
using System.Collections.Generic;
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
    private GameObject playerTwoClaw;
    private GameObject playerThreeClaw;
    private GameObject playerFourClaw;
    private GameObject CollectableObject;
    private int defaultSpeed = 50;
    private Animator playerOneAnim;
    private bool isSpeedBuffed;    
    private bool objectGrabbed = false;
    public RoundHandler scoreTrackingScript;
    public string playerIdentifier;

    private int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Object tagged Player 
        playerOneClaw = GameObject.FindGameObjectWithTag("p1 Player");
        //playerTwoClaw = GameObject.FindGameObjectWithTag("Player Two");
        //playerThreeClaw = GameObject.FindGameObjectWithTag("Player Three");
        //playerFourClaw = GameObject.FindGameObjectWithTag("Player Four");
        
        scoreTrackingScript = GameObject.FindGameObjectWithTag("RoundHandler").GetComponent<RoundHandler>();
    }

    public void moveClaw()
    {
        //This code gets the X and Z input and moves the Player object with it.
        //float Xmovement = Input.GetAxis("Hor_" + pName);
        float Xmovement = Input.GetAxis("Hor_" + playerIdentifier);
        float Ymovement = Input.GetAxis("Vert_" + playerIdentifier);
        Vector3 movement = new Vector3(Xmovement, 0, Ymovement);
        //movement.Normalize();
        parentObject.transform.Translate(movement * clawMoveSpeed * Time.deltaTime);
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
        // If the player presses G, run the animation that makes the Claw drop down.
        if (Input.GetKeyDown(KeyCode.G))
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
                grabCooldown = 100;
                objectGrabbed = true;
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


        if(other.gameObject.tag == "Player Two")
             
            {
                Debug.Log("Collided with another claw!");
                other.gameObject.GetComponent<FixedJoint>();
                // destroy the fixed joint on the other claw's object 
                Destroy(other.gameObject.GetComponent<FixedJoint>());
                // destroy the fixed joint on this claw's object
                Destroy(gameObject.GetComponent<FixedJoint>());
            }

         if(other.gameObject.tag == ("Wall"))
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
            points += value;
        }

        public int getPoints()
        {
            return points;
        }

}