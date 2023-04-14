using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{

	// Coordinates of player object 
	public Transform playerControllerCoordinates;
	// Vector that determines how far away from player
	// the grabbable object will be placed

	public Vector3 displacementVector;

	// These are the array of prefabs that can be instantiated
	// It is important that corresponding prefabs have the same index when
	// set up in the editor.
	// For example, if MudPigAI is at index 7, MudPigGrabbable must also be at index 7
	// Otherwise, in scene transition animal types will be mismatched  
	public GameObject[] grabbableAnimals;
	public GameObject[] AIAnimals;

	// Lists to keep track of Animals in the game
	// The value they contain is the prefab index they belong to
	private List<int> LiveAIsIndicies;
	private List<int> LiveGrabbablesIndicies;

	// List that tracks the game objects themselves
	private List<GameObject> LiveAnimalObjects;
	
	// List to keep track of animal positions
	private List<Vector3> LastPosition; 

	// Start is called before the first frame update
	void Start(){

		// init our lists to track objects currently in the world
		LiveAIsIndicies = new List<int>();
		LiveGrabbablesIndicies = new List<int>();
		LiveAnimalObjects = new List<GameObject>();
		LastPosition = new List<Vector3>();

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
		int numberOfAnimals = LiveAnimalObjects.Count;
		for(int i = 0; i < numberOfAnimals; i++){
			if(LiveAnimalObjects[i] == null){
				LiveAnimalObjects.Remove(LiveAnimalObjects[i]);
				break;
			}
			else{
				Debug.Log("updating last position");
				LastPosition[i] = LiveAnimalObjects[i].transform.position;
			}
		}

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
			// Add AI animal prefab type index to LiveAI
			LiveAIsIndicies.Add(index);
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
			// Add Grabbable animal; prefab index to LiveGrabbables
			LiveGrabbablesIndicies.Add(index);
			LiveAnimalObjects.Add(go);
			LastPosition.Add(go.transform.position);
		}else{
			Debug.Log("Prefab is null. Something is wrong. Fix it!");
		}
	}


	public void SwapGrabbablesWithAI(){
		
		Debug.Log("SwapGrabbablesWith AI invoked...");
		// 'i' corresponds to the index of the object in the list
		// of grabbable animals that are active in the game
		// 'index' corresponds to the animal prefabs index in terms
		// of how it is ordered relative to the other animal prefabs
		// in the prefab lists
		Debug.Log("Swapping " + LiveGrabbablesIndicies.Count + "Animals.");
		for(int i= 0 ; i < LiveGrabbablesIndicies.Count; i++){
			int index = LiveGrabbablesIndicies[i];
			Vector3 pos = LastPosition[i];
			GameObject prefab = AIAnimals[index];
			CreateAIObject(prefab, pos, index);
			LiveAIsIndicies.Add(index);
			//LiveGrabbablesIndicies.Remove(index);
		}

	}

}// end of class
