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
    


    //Moving back and forth Variables
    public Transform stopperLeft, stopperRight;
    float MIN_X, MAX_X;
    bool goingLeft = true;

    //Animation Variables
    private bool hasAnimations = true; //using this as default for now
    int coroutineCount = 0; // this protects against starting too many coroutines
    int MAX_ROUTINES = 10;
//
    bool isGameStarted = false; 
    // Start is called before the first frame update
    void Start()
    {
        if( mesh.GetComponent<Animator>() != null){
            anim = mesh.GetComponent<Animator>();
            hasAnimations = true;
        }
        goalieState = State.idle;
        isGameStarted = true;
        MAX_X = stopperLeft.position.x;
        MIN_X = stopperRight.position.x;
        

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
                    if(hasAnimations){ 
                        anim.SetBool("isReady", true);
                    }
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
                
                 if(coroutineCount < MAX_ROUTINES ){
                    coroutineCount++;
                    if(goingLeft){
                        StopCoroutine(moveRight());
                        
                        StartCoroutine(moveLeft());
                    }else{
                        StopCoroutine(moveLeft());
                        
                        StartCoroutine(moveRight());
                    }
                 }

                 if(hasAnimations) {
                    anim.SetBool("isAlert",true);
                    anim.SetBool("goLeft", goingLeft);
                 }

                 

                //change state back to ready if you leave the distance
                if(distance > exitAltertStateDistance){
                    goalieState = State.ready;
                    coroutineCount = 0;
                    if(hasAnimations) {
                        anim.SetBool("isAlert", false);
                        anim.SetBool("isReady", true);
                        StopCoroutine(moveLeft());
                        StopCoroutine(moveRight());
                         
                    }
                    
                }
                else{
                }
                break;
        }
    }

    public void TimerDone()
    {
        isGameStarted = true;
        Debug.Log("DEBUG: Timer done!");
    }


   public IEnumerator moveLeft(){
     Debug.Log("In moveLeft");
       while(mesh.transform.position.x > MIN_X){
           Debug.Log("moving left");
            mesh.transform.position = new Vector3(mesh.transform.position.x - 0.03f , mesh.transform.position.y, mesh.transform.position.z);
            yield return new  WaitForSeconds(0.1f);
       }
       goingLeft = false;
       coroutineCount = 0;
    }

   public IEnumerator moveRight(){
      Debug.Log("In moveRight");
     if(hasAnimations){
                        anim.SetBool("goLeft",false);
             }
       while(mesh.transform.position.x < MAX_X){
        Debug.Log("moving right");
            mesh.transform.position = new Vector3(mesh.transform.position.x + 0.03f , mesh.transform.position.y, mesh.transform.position.z);
            yield return new  WaitForSeconds(0.1f);
       }
       goingLeft = true;
       coroutineCount = 0;
    }



// function that makes goalie stand in front of the goal, right now i am not using it! but it works for what it is 
    void  guard(){
    
        float difference = player.transform.position.x- mesh.transform.position.x; 
        float absolute_difference = Mathf.Abs(difference);
        if(absolute_difference >= 0.5 ){
         Vector3 direction = ((player.transform.position - mesh.transform.position).normalized);
         mesh.transform.position = new Vector3(mesh.transform.position.x + direction.x, mesh.transform.position.y,mesh.transform.position.z);
        }
        
    }

} // end of class
