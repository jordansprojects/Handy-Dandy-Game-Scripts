using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    // table to associate prefab types with eachother
    private Hashtable prefabTable;

    // Coordinates of player object 
    public Transform playerControllerCoordinates;
    // Vector that determines how far away from player
    // the grabbable object will be placed
  
    public Vector3 displacementVector;

    // These are the array of prefabs that can be instantiated
    public GameObject[] grabbableAnimals;
    public GameObject[] AIAnimals;

    // These are the tables to keep track of instantiated objects
    
    // For created AI, the key is in the index of its prefab type (should be the same index for the corresponding grabbable type)
    // and the value is the object itself. 
    private Hashtable createdAI;

    // For created Grabbles, the key is in the index of its prefab type (should be the same index for the corresponding AI type)
    // and the value is the object itself. 
    private Hashtable createdGrabbables;
    
    // Start is called before the first frame update
    void Start()
    {
	// instaniate our tables to store objects currently in the world
	createdAI = new Hashtable();
	createdGrabbables = new Hashtable();
	prefabTable = new Hashtable();	    
	

	// both of our prefab lists should be of equal length
	if(grabbableAnimals.Length != AIAnimals.Length){
		Debug.Log("AI Animal List  must be the same length as Grabbable Animal List.");
	}

	// Populate Hash Table with grabbable animals as key and their AI counterparts as value 
	int numAnimals = grabbableAnimals.Length;
	for (int i = 0; i < numAnimals; i++){
		prefabTable.Add(grabbableAnimals[i], AIAnimals[i]);
	}
    
    // Lets call our tester function
    // this will be replaced once we connect Zachs lovely store! 
     testAnimalSpawner();

     // lets test the swap method too
     //Commented out for now, bc I am just demonstrating the grabbable spawn!
     //SwapGrabbablesWithAI();
    }// end of start

    // Update is called once per frame
    void Update()
    {
        
    }


    // This is just a test function, spawns each prefab type into the world
    void testAnimalSpawner(){
    	for (int i = 0; i < grabbableAnimals.Length; i++ ){
		CreateGrabbableObject(grabbableAnimals[i], i);
	}
    }

    // CreateAIObject instantiates AI from prefabs 
    // @param GameObject prefab : The type of prefab being instantiated
    // @param int index :  The index of the object in the AI prefab array
 
    public void CreateAIObject(GameObject prefab, Vector3 spawnLoc, int index){
    	if (prefab != null){
		GameObject go = (GameObject)Instantiate(prefab, spawnLoc, Quaternion.identity);
		createdAI.Add(index, go);
	} else{
		Debug.Log("Prefab is null. Something is wrong. Fix it!");
	}
    }

    // CreateGrabbableObject instantiates grabbable Animals from prefab 
    // @param GameObject prefab : The type of prefab being instantiated
    // @param int index :  The index of the object in the grabbable prefab array
    public void CreateGrabbableObject(GameObject prefab, int index){
    	if( prefab != null){
		Vector3 spawnLoc = playerControllerCoordinates.position + displacementVector;
		GameObject go = (GameObject)Instantiate(prefab, spawnLoc, Quaternion.identity);
		createdGrabbables.Add(index, go);
	}else{
		Debug.Log("Prefab is null. Something is wrong. Fix it!");
	}
    }


   public void SwapGrabbablesWithAI(){
	Debug.Log("SwapGrabbablesWith AI invoked...");
   	foreach (int key in createdGrabbables.Keys ){
			GameObject obj = (GameObject)createdGrabbables[key];
			Vector3 spawnLoc = obj.transform.position;
			CreateAIObject(AIAnimals[key] , spawnLoc, key);
	}
   
   }









}// end of class
