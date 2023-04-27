using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;
using EmeraldAI;
using System.Linq;

public class SceneTransistionManager: MonoBehaviour{

	public UnityEvent onNightSceneLoaded;
	public UnityEvent beforeNightSceneLoaded;


	public UnityEvent onDaySceneLoaded;
	public UnityEvent beforeDaySceneLoaded;

	// This tells us what the scenecoad means
	private const int DAY = 0;
	private const int NIGHT = 1;  


	public string nightScene;
	public string dayScene;
	public int sceneCode =DAY; // this tells you which scene to load. 0 -> night, 1 -> back to day

	// Starting Spawn Location for Daytime Animals
	public Transform spawnLoc;

	// These are the array of prefabs that can be instantiated
	// It is important that corresponding prefabs have the same index when
	// set up in the editor.
	// For example, if MudDayTime is at index 7, MudPigNightTime must also be at index 7
	// Otherwise, in scene transition animal types will be mismatched  
	public GameObject[] dayAnimals;
	public GameObject[] nightAnimals;

	// Lists to keep track of Animals in the game
	// The value they contain is the prefab index they belong to
	private List<int> AnimalIndicies;

	// List that tracks the GameObjects themselves
	private List<GameObject> AnimalObjects;

	// List to keep track of animal positions
	private List<Vector3> LastPosition; 



	// lists for AI storage
	List<GameObject> totalAIs;
	List<GameObject> enemies;
	List<GameObject> allies;


	// Coroutine references for starting and stopping
	private Coroutine checkListCoroutine;

	// Start is called before the first frame update
	void Start()
	{
		// init lists 
		enemies = new List<GameObject>();
		allies  = new List<GameObject>();

		// init our lists to track of animals currently in the world
		AnimalIndicies = new List<int>();
		AnimalObjects = new List<GameObject>();	
		LastPosition = new List<Vector3>();


	}

	// This is invoked during an event by the OnSceneLoaded for night mode
	public void StartNightCoroutine(){
		Debug.Log("Start night coroutine");
		checkListCoroutine = StartCoroutine(CheckClearedStageCoroutine());
	}

	private IEnumerator CheckClearedStageCoroutine()
	{
		while (true)
		{
			// Wait for 10 seconds
			yield return new WaitForSeconds(3f);

			Debug.Log("There are " + enemies.Count + " enemies left.");
			// loop through all enemies
			for ( int i = 0; i < enemies.Count; i++){
				// grab emerald ai system component from enemy
				EmeraldAISystem EmeraldComponent = enemies[i].GetComponent<EmeraldAISystem>();
				if(EmeraldComponent.IsDead){
					// Remove enemy from list because hes now dead!
					enemies.Remove(enemies[i]);
				}
			} // 

			// Check if the enemy list is empty
			if (enemies.Count == 0)
			{
				// Unsubscribe from the nighttime sceneLoaded event
				SceneManager.sceneLoaded -= OnSceneLoaded;
				beforeDaySceneLoaded.Invoke();
				// Load Day Scene
				Debug.Log("Invoking the loading of day scene");
				SceneManager.LoadScene(dayScene);

				// Subscribe to the daytime sceneLoaded event 
				SceneManager.sceneLoaded += OnSceneLoaded;

				//set sceneCode to day	
				sceneCode = DAY;

				// Stop the coroutine
				StopCoroutine(checkListCoroutine);
			}
		}
	}


	// Update is called once per frame
	void Update(){}


	void OnSceneLoaded( Scene scene, LoadSceneMode mode){
		if(scene.name == nightScene){
			//invoke event for loaded night scene
			onNightSceneLoaded.Invoke();
			Debug.Log("Night loaded.");

			// switch sceneCode to 1
			sceneCode = NIGHT; 
		}else if(scene.name == dayScene){
			// invoke event for loaded day scene
			onDaySceneLoaded.Invoke();
			Debug.Log("Day Loaded. ");
			//switch sceneCode to 0, 
			sceneCode= DAY ; 		
		}
	}




	//if we go with GUI implementation this can be replaced
	// with a " on button / trigger pressed" type event
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && sceneCode == DAY){

			//Unsubscribe from daytime sceneLoaded event
			SceneManager.sceneLoaded -= OnSceneLoaded; 

			// Invoke event to prepare for nighttime scene load
			beforeNightSceneLoaded.Invoke();

			// Load night scene
			SceneManager.LoadScene(nightScene);

			// Subscribe to nighttime sceneLoaded event
			SceneManager.sceneLoaded += OnSceneLoaded; 
		}
	}


	/***************************************************************************
	 * This is responsible for counting all instances of enemies in the scene
	 * It is copied over from ShopManager. We use it here for determining 
	 * whether the player has won 
	 **************************************************************************/
	public void populateEnemyArray(){
		// collects all objects with this tag
		totalAIs = GameObject.FindGameObjectsWithTag("AI").ToList();
		foreach (GameObject ai in totalAIs){
			EmeraldAISystem sys = ai.GetComponent<EmeraldAISystem>();
			//Debug.Log("faction # : " + sys.CurrentFaction);
			if ( sys.CurrentFaction == 0){
				enemies.Add(ai);
			}else{
				allies.Add(ai);
				// Here, we also probably want to store the aly index  # in a list 
			} 
		} // end of loop
	}
	// loops through every animal and then 
	// tracks their positon
	public void updatePosition(){
		int numberOfAnimals = AnimalObjects.Count;
		for(int i = 0; i < numberOfAnimals; i++){
			if(AnimalObjects[i] == null){
				break;
			}
			else{
				LastPosition[i] = AnimalObjects[i].transform.position;
			}
		}
	}



	// CreateAIObject instantiates AI from prefabs 
	// @param GameObject prefab : The type of prefab being instantiated
	// @param int index :  The index of the object in the AI prefab array
	// @param Vector3 spawnLoc : Location of where the object should be spawned
	public void CreateAIObject(GameObject prefab,int index, Vector3 spawnLoc){
		if (prefab != null){
			GameObject go = (GameObject)Instantiate(prefab, spawnLoc, Quaternion.identity);
			// Add AI animal prefab type index list
			AnimalIndicies.Add(index);
			// Add object itself to object list
			AnimalObjects.Add(go);
		} else{
			Debug.Log("Prefab is null. Something is wrong. Fix it!");
		}
	}
	// CreateAIObject instantiates AI from prefabs 
	// @param GameObject prefab : The type of prefab being instantiated
	// @param int index :  The index of the object in the AI prefab array
	public void CreateAIObject(GameObject prefab,int index){
		if (prefab != null){
			GameObject go = (GameObject)Instantiate(prefab, spawnLoc.position, Quaternion.identity);
			// Add AI animal prefab type index list
			AnimalIndicies.Add(index);
			// Add object itself to object list
			AnimalObjects.Add(go);
			// Add entry to last position list
			LastPosition.Add(go.transform.position);
		} else{
			Debug.Log("Prefab is null. Something is wrong. Fix it!");
		}
	}
	public void checkForDeaths(){
		// Remove dead animals from list before swap is invoked
		for(int i = 0; i< AnimalObjects.Count; i++){
			EmeraldAISystem sys = AnimalObjects[i].GetComponent<EmeraldAISystem>();
			if (sys.IsDead){
				AnimalObjects.RemoveAt(i);
				AnimalIndicies.RemoveAt(i);
				LastPosition.RemoveAt(i);
			}
		}	
	}


	public void SwapDayAnimalsWithNightAnimals(){		
		Debug.Log("SwapGrabbablesWith AI invoked...");
		// 'i' corresponds to the index of the object in the list
		// of grabbable animals that are active in the game
		// 'index' corresponds to the animal prefabs index in terms
		// of how it is ordered relative to the other animal prefabs
		// in the prefab lists
		Debug.Log("Swapping " + AnimalObjects.Count + "Animals.");
		for(int i= 0 ; i < AnimalObjects.Count; i++){
			int index = AnimalIndicies[i];
			Vector3 pos = LastPosition[i];
			GameObject prefab = nightAnimals[index];
			// Overwrite entry with new object
			AnimalObjects[i] =(GameObject)Instantiate(prefab, pos, Quaternion.identity);

		}
	}

	public void SwapNightAnimalsWithDayAnimals(){
		Debug.Log("Swap Night Animals with Day Animals invoked...");

		// 'i' corresponds to the index of the object in the list
		// of grabbable animals that are active in the game
		// 'index' corresponds to the animal prefabs index in terms
		// of how it is ordered relative to the other animal prefabs
		// in the prefab lists
		Debug.Log("Swapping " + AnimalObjects.Count + "Animals.");
		for(int i= 0 ; i < AnimalObjects.Count; i++){
			int index = AnimalIndicies[i];
			Vector3 pos = LastPosition[i];
			GameObject prefab = dayAnimals[index];
			AnimalObjects[i] =(GameObject)Instantiate(prefab, pos, Quaternion.identity);


		}
	}
	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(spawnLoc.position, 0.5f);

	}


}//end of class
