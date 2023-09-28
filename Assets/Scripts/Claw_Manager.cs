using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Claw_Manager : MonoBehaviour
{

    private GameObject playerOneClaw;
    private GameObject CollectableObject;
    [SerializeField]  int clawMoveSpeed = 5;
    [SerializeField] int grabCooldown = 0;
    private bool objectGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playerOneClaw == null)
        {
            playerOneClaw = GameObject.FindGameObjectWithTag("PlayerOne");
        }
    }

    public void moveClaw()
    {
        float Xmovement = Input.GetAxis("Horizontal");
        float Ymovement = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(Xmovement, Ymovement, 0);
        movement.Normalize();
        GetComponent<Rigidbody>().velocity = movement * clawMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveClaw();
        dropObject();
        if(grabCooldown > 0 && grabCooldown <= 100 )
        {
            grabCooldown--;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Collectable")
         {
            //Debug.Log("Collided with Collectable");

            if ( grabCooldown == 0 && !objectGrabbed)
            {
                FixedJoint grabbingJoint = gameObject.AddComponent<FixedJoint>();
                grabbingJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                objectGrabbed = true;
               // Debug.Log("Grabbed Object");
            }


        }
    }

    private void dropObject()
    {
        if (objectGrabbed)
        {
            if(Input.GetKeyDown(KeyCode.Space)){
                Destroy(GetComponent<FixedJoint>());
                objectGrabbed = false;
                grabCooldown = 100;
                Debug.Log("Dropped object");

            }
        }
    }
   

}
