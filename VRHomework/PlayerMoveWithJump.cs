using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    //movement physics provided by Unity can be accessed from the rigidbody component
    //give rigidbody component in the editor to the game objects that need the physics
    Rigidbody playerRigidbody;

    public float movespeed = 1.0f;

    //always have a check so that you can turn on/off the debug info from different scripts easily
    public bool printDebugInfo = false;

    //saves the mouse input to rotate the player perspective
    public Vector2 turn;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame (around 60-120 times per second depending on your game speed)
    // Think this update call as a big infinite loop
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(printDebugInfo)
                Debug.Log("User pressed W, go forward");
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            playerRigidbody.AddForce(transform.forward * movespeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (printDebugInfo)
                Debug.Log("User pressed S, go backward");
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            playerRigidbody.AddForce(-transform.forward * movespeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (printDebugInfo)
                Debug.Log("User pressed A, go left");
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            playerRigidbody.AddForce(-transform.right *movespeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (printDebugInfo)
                Debug.Log("User pressed D");
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            playerRigidbody.AddForce(transform.right *movespeed);
        }
        // Added jumping functionality
        
        if(Input.GetKey(KeyCode.Space)){
            Debug.Log("User pressed Space");
            // //Apply a force to this Rigidbody in direction of this GameObjects up axis
            playerRigidbody.AddForce(transform.up *movespeed);
        }
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");//rotate up/down

        transform.localRotation = Quaternion.Euler(-turn.y, turn.x,0);
    }
    


}
