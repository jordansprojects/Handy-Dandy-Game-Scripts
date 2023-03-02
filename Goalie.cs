using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
WITH SUBMISSION PROVIDE NOTE THAT THE PROFESSOR GAVE US THE GREEN LIGHT TO HAVE THE GUARD
BE ACTIVATED INTO READY STATE BASED ON A TIMER INSTEAD OF A DISTANCE FROM THE PLAYER

*/


public class Goalie : MonoBehaviour
{
    //Using state machine structure to plan and organize your code
    //will make it a lot easier for you to expand and maintain your event-
    //driven programming scripts
    public enum State
    {
        idle, // completely unthreatened. not moving 
        ready, // moving a little bit, preparing for attempt to score
        alert // moving fast, knows player is trying to score      
    }
    // Essential Goalie vars
    private State goalieState;
    public GameObject player;
    public GameObject mesh;
    Animator anim; 
    bool isAlert; //used to control animation, is dependency for coroutine

    //goalie idle state variables
    public float readytoAlertDistance = 14.0f;
    private float speed=1.0f;
    // private Vector3 startPos;
    //goalie alert state variables
    public float exitAltertStateDistance = 20.0f; 
    private float alertSpeed = 0.5f;


//
    bool isGameStarted = false; 
    // Start is called before the first frame update
    void Start()
    {
        anim = mesh.GetComponent<Animator>();
        goalieState = State.idle;
        StartCoroutine(guard());
    }

    // Update is called once per frame
    void Update()
    {

        float distance  = Vector3.Distance(mesh.transform.position, player.transform.position) ;
        //use switch case to organize code based on your state machine drawing
        switch (goalieState) {
            case State.idle:
                Debug.Log("DEBUG: In Idle");
                //change state condition, if 
                if (isGameStarted)
                {
                    goalieState = State.ready;
                    anim.SetBool("isReady", true);
                }

                break;
            case State.ready:
                Debug.Log("DEBUG: In Ready");
                // moves back and forth, left to right 
                mesh.transform.LookAt(player.transform);
                //Change state conditions
                //if you are gone
                if (distance < readytoAlertDistance)
                    goalieState = State.alert;
                break;
            case State.alert:
                 Debug.Log("DEBUG: In Alert");
                 anim.SetBool("isAlert",true);

                 mesh.transform.LookAt(player.transform);

                //change state back to ready if you leave the distance
                if(distance > exitAltertStateDistance){
                    goalieState = State.ready;
                    isAlert = false;
                    
                }
//
                else{
                   isAlert=true;
                   guard();
                }
                anim.SetBool("isAlert", isAlert);
                break;
        }
    }

    public void TimerDone()
    {
        isGameStarted = true;
        Debug.Log("DEBUG: Timer done!");
    }

    void  guard(){
        Debug.Log("DEBUG: in guard!");
        float difference = player.transform.position.x- mesh.transform.position.x; 
        float absolute_difference = Mathf.Abs(difference);
        if(absolute_difference >= 0.5 ){
                    Vector3 direction = ((player.transform.position - mesh.transform.position).normalized);
                    if(difference > 0 ) {
                        anim.SetBool("walkLeft", false);
                    }else{
                        anim.SetBool("walkLeft", true);
                    }

        mesh.transform.position = new Vector3(mesh.transform.position.x + direction.x, mesh.transform.position.y,mesh.transform.position.z);
        
        }
    }





} // end of class
