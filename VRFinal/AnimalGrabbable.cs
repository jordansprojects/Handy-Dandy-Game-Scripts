using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using EmeraldAI;

public class AnimalGrabbable : MonoBehaviour
{
   // public editor variables 
    public bool isGrabbed;

    // This is important for rotating the animal properly 
    // when it is being picked up and released 
    public Transform animalTransform;	
    // Set to the animals animator, so that it can be disabled
    // when an animal is picked up, and renabled when the animal is put down
    public Animator animator; 

    // Set to the animal prefabs navmesh agent, so that it can be disabled
    // when the animal is picked up, and renabled when the animal is put 
    // down
    public UnityEngine.AI.NavMeshAgent navmesh;


    // Default rotation setting of the object, so that it can be restored
    // after the object has been moved
    // I suggest (0,0,0) unless your prefab has some hierarchy issue
    // where its upright when it has some strange rotation vector values
    public Quaternion default_rot; 

    // Start is called before the first frame update
    void Start()
    {
	// Tempory fix to resolve the bug where the animal walks in place.
	navmesh.velocity = transform.forward * navmesh.speed;
    }

    // Update is called once per frame
    void Update()
    {
	// This is just for debugging, we do not want
	// this function in update() !!
	checkForGrab();

    }

    
    public void checkForGrab(){
    	if(isGrabbed == true){
		onGrab();
	}else{
		onDrop();
	}
    }

    public void onGrab(){
    	//disable animation controller
	animator.enabled = false;
	//Stop navmesh agent 
	navmesh.isStopped = true;
	
    }

    public void onDrop(){
    	// Renable animation controller
	animator.enabled = true;
 	//Restart navmesh agent
	navmesh.isStopped = false;
	// This is to restore the sheeps velocity lost from stopping
    	navmesh.velocity = transform.forward * navmesh.speed;
    
	//set rotation to default position 
	transform.rotation = default_rot; 

    
    }
}
