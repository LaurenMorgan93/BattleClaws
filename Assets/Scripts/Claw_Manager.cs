using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Claw_Manager : MonoBehaviour
{

    private GameObject playerOneClaw;
    private Animator playerOneAnim;
    private GameObject CollectableObject;
    [SerializeField]  int clawMoveSpeed;
    [SerializeField] int grabCooldown;
    private bool objectGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the Object tagged Player One
        playerOneClaw = GameObject.FindGameObjectWithTag("Player One");
    }

    public void moveClaw()
    {
        //This code gets the X and Z input and moves the Player object with it.
        float Xmovement = Input.GetAxis("Horizontal");
        float Ymovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(Xmovement, 0, Ymovement);
        movement.Normalize();
        transform.Translate(movement * clawMoveSpeed * Time.deltaTime);
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
            Debug.Log("Dropped Object!");
            
        }
    }
}





private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag == "Collectable")
    {
         Debug.Log("Trigger entered with: " + other.gameObject.name);
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
}

}
