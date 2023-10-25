using TMPro;
using UnityEngine;

public class Claw_Manager : MonoBehaviour
{
    [SerializeField] public float buffCooldown;
    [SerializeField] private int grabCooldown;
    public int clawMoveSpeed;
    public bool doublePointsBuffActive;
    public GameObject parentObject;
    public RoundHandler scoreTrackingScript;
    public string playerIdentifier;

    private AudioHandler audioScript;

    public int points;
    private readonly float maxX = 140f; // Set your maximum X position
    private readonly float maxZ = 235f; // Set your maximum Z position

    // Define the boundaries for player movement
    private readonly float minX = -150f; // Set your minimum X position
    private readonly float minZ = -60f; // Set your minimum Z position
    private GameObject CollectableObject;
    private int defaultSpeed = 50;
    public GameObject heldObject;
    private bool isSpeedBuffed;
    public bool objectGrabbed;
    private Animator playerOneAnim;
    private GameObject playerOneClaw;
    public GameObject clawModel;

    private TMP_Text scoreUI;

    public sbyte isInverted = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Find the Object tagged Player 
        playerOneClaw = gameObject;
        scoreUI = GameObject.FindGameObjectWithTag(playerIdentifier + " Score").GetComponent<TMP_Text>();
        scoreUI.text = "000000";

        scoreTrackingScript = GameObject.FindGameObjectWithTag("RoundHandler").GetComponent<RoundHandler>();

        audioScript = FindObjectOfType<AudioHandler>();
    }

    // Update is called once per frame
    private void Update()
    {
        moveClaw();
        PlayerGrab();
        if (grabCooldown > 0) grabCooldown--;

        if (isSpeedBuffed)
        {
            buffCooldown -= Time.deltaTime;

            if (buffCooldown <= 0)
            {
                isSpeedBuffed = false;
                clawMoveSpeed = defaultSpeed;
            }
        }

        if (doublePointsBuffActive)
        {
            buffCooldown -= Time.deltaTime;
            if (buffCooldown <= 0) doublePointsBuffActive = false;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.EndsWith("Player"))
        {
            audioScript.PlaySoundEffect("Crash");
            Debug.Log("Collided with another claw!");
            other.gameObject.GetComponent<FixedJoint>();
            // destroy the fixed joint on the other claw's object 
            Destroy(other.gameObject.GetComponent<FixedJoint>());
            if (other.gameObject.GetComponent<Claw_Manager>().heldObject != null)
            {
                if (scoreTrackingScript.isDrawRound)
                {
                    other.gameObject.GetComponent<Claw_Manager>().heldObject.GetComponent<SpecialCollectable>()
                        .setHeld(false);
                }
                var otherObject = other.gameObject.GetComponent<Claw_Manager>().heldObject;
                other.gameObject.GetComponent<Claw_Manager>().heldObject.GetComponent<Collectables>().activateSuperCharge();
                otherObject.transform.parent = null;
                otherObject.GetComponent<Rigidbody>().useGravity = true;
                otherObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                otherObject.GetComponent<Rigidbody>().AddForce(Vector3.down* 100, ForceMode.Impulse);
                
                other.gameObject.GetComponent<Claw_Manager>().objectGrabbed = false;
                other.gameObject.GetComponent<Claw_Manager>().heldObject = null;
            }

            // destroy the fixed joint on this claw's object
            if (heldObject != null)
            {
                if (heldObject && scoreTrackingScript.isDrawRound)
                {
                    heldObject.GetComponent<SpecialCollectable>().setHeld(false);
                    
                }
                print("let go");
                heldObject.transform.parent = null;
                heldObject.GetComponent<Collectables>().activateSuperCharge();
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().AddForce(Vector3.down* 100, ForceMode.Impulse);
                print("DROPPED!");
                objectGrabbed = false;
                heldObject = null;
            }
        }

        if (other.gameObject.CompareTag("Wall")) Destroy(gameObject.GetComponent<FixedJoint>());
    }

    public void moveClaw()
    {
        //This code gets the X and Z input and moves the Player object with it.
        //float Xmovement = Input.GetAxis("Hor_" + pName);
        
        
        var Xmovement = Input.GetAxis("Hor_" + playerIdentifier) * isInverted;
        var Ymovement = Input.GetAxis("Vert_" + playerIdentifier) * isInverted;
        if (Mathf.Abs(Xmovement) <= 0.2 && Mathf.Abs(Ymovement) <= 0.2) return;
        var movement = new Vector3(Xmovement, 0, Ymovement);
        // Clamp the player's position within the boundaries
        parentObject.transform.Translate(movement * clawMoveSpeed * Time.deltaTime);
        var newPosition = parentObject.transform.position + movement * clawMoveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
        // Update the player's position
        parentObject.transform.position = newPosition;
    }

    private void PlayerGrab()
    {
        
        // If the player presses the button, run the animation that makes the Claw drop down.
        if (Input.GetKeyDown("joystick " + playerIdentifier[1] + " button 0"))
        {
            
            if (!objectGrabbed && grabCooldown <= 0)
            {
                print("GRABBING!");
                playerOneAnim = playerOneClaw.GetComponent<Animator>();
                playerOneAnim.SetTrigger("Grab");
                grabCooldown = 100;
            }
            else if (objectGrabbed && heldObject)
            {
                if (heldObject && scoreTrackingScript.isDrawRound)
                {
                    heldObject.GetComponent<SpecialCollectable>().setHeld(false);
                    
                }
                print("let go");
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                heldObject.GetComponent<Rigidbody>().AddForce(Vector3.down* 100, ForceMode.Impulse);
                print("DROPPED!");
                objectGrabbed = false;
                heldObject = null;
                // Drop the object by destroying the FixedJoint on this object.
                var fixedJoints = GetComponentsInChildren<FixedJoint>();
                foreach (var joint in fixedJoints) {Destroy(joint);}
            }
        }
    }

    public void setSpeed(int value)
    {
        defaultSpeed = clawMoveSpeed;
        clawMoveSpeed = value;
        buffCooldown = 10;
        isSpeedBuffed = true;
    }

    public void addPoints(int value)
    {
        audioScript.PlaySoundEffect("Score");
        points += value;
        scoreUI.text = points.ToString().PadLeft(6, '0');
    }

    public int getPoints()
    {
        return points;
    }

    private GameObject sendRay()
    {
        RaycastHit hit;

        if (Physics.BoxCast(gameObject.transform.position, new Vector3(0.9f, 0.9f, 0.9f), Vector3.down, out hit) &&
            (hit.collider.CompareTag("Collectable") || hit.collider.CompareTag("MysteryBox")))
            return hit.collider.gameObject;

        return null;
    }

    public void checkGrab()
    {
        var currentTarget = sendRay();

        try
        {
            if (currentTarget.CompareTag("Collectable") || currentTarget.CompareTag("MysteryBox"))
            {
                if (currentTarget.TryGetComponent<SpecialCollectable>(out SpecialCollectable specialScript))
                {
                    specialScript.holdingPlayer = this.gameObject;
                    specialScript.setHeld(true);
                    
                }
                currentTarget.transform.SetParent(clawModel.transform);
                currentTarget.GetComponent<Rigidbody>().useGravity = false;
                objectGrabbed = true;
                heldObject = currentTarget;

                if (currentTarget.CompareTag("MysteryBox"))
                {
                    var randomScript = currentTarget.GetComponent<RandomEffect>();
                    randomScript.lastPlayerGrab = this.gameObject;
                    randomScript.PlayerManagerScript = this;
                }
                else
                {
                    var collectableScript = currentTarget.GetComponent<Collectables>();
                    collectableScript.lastPlayer = this.gameObject;
                }
            }
        }
        catch
        {
        }
    }
}