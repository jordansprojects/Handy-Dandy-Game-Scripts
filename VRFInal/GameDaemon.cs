using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;
using EmeraldAI;
using System.Linq;
using System; //for System.Math
//Serializeable so ShopManager can reference it
[System.Serializable]
public class GameDaemon: MonoBehaviour{


	public ShopManager sm;
	public UnityEvent onNightSceneLoaded;
	public UnityEvent beforeNightSceneLoaded;


	public UnityEvent onDaySceneLoaded;
	public UnityEvent beforeDaySceneLoaded;

	// This tells us what the scenecode means
	private const int DAY = 0;
	private const int NIGHT = 1;  

	// the n-th level without having to work up to it one-by-one
	public int startLevel =0;

	// Distance from bed to where monsters are spawned
	// This is a float, because the spawn points are points on a circle,
	// where this distance from the bed is our radius. 
	public float monsterSpawnRadius = 10.0f;
	
	// Strings of the scenes we are transistioning back and forth from
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

	// Monster prefab list for spawning monsters
	public GameObject[] monsters;
	

	// Lists to keep track of Animals in the game
	// The value they contain is the prefab index they belong to
	private List<int> AnimalIndicies;

	// List that tracks the GameObjects themselves
	private List<GameObject> AnimalObjects;

	// List to keep track of animal positions
	private List<Vector3> LastPosition; 



	// lists for storing AIs in the world
	List<GameObject> totalAIs;
	List<GameObject> enemies;
	List<GameObject> allies;


	//Refernse to storeui so the canvas can be hidden during the night
	public GameObject StoreUI;

	// Coroutine references for starting and stopping
	private Coroutine checkListCoroutine;
	private Coroutine checkAnimalDeathsCoroutine;

	// internal level for scaling the # of enemies spawned
	private int level;

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
		
		//Set starting Level
		level = startLevel;

	}

	// This is invoked during an event by the OnSceneLoaded for night mode
	public void StartNightCoroutine(){
		Debug.Log("Start night coroutines");
		checkListCoroutine = StartCoroutine(CheckClearedStageCoroutine());
	}



	/***********************************************
	 * This probably doesnt need to be a coroutine
	 * 	
	 * ************************************************/
	private IEnumerator CheckAnimalDeathsCoroutine(){
		// Wait for 3 seconds
		yield return new WaitForSeconds(0.25f);
		// Loop through all animals
		for ( int i = 0; i < AnimalObjects.Count; i++){
			// grab emerald ai system component from enemy
			EmeraldAISystem EmeraldComponent = AnimalObjects[i].GetComponent<EmeraldAISystem>();
			if(EmeraldComponent.IsDead){
				Debug.Log("Dead animal reported.");
				// Remove animal from list because hes now dead!
				AnimalObjects.RemoveAt(i);
				AnimalIndicies.RemoveAt(i);
				LastPosition.RemoveAt(i);
			}else{
				Debug.Log("This animal is alive.");
			}
		} 


	}

	private IEnumerator CheckClearedStageCoroutine()
	{
		while (true)
		{
			// Wait for 3 seconds
			yield return new WaitForSeconds(3f);

			Debug.Log("There are " + enemies.Count + " enemies left.");
			// loop through all enemies
			for ( int i = 0; i < enemies.Count; i++){
				// grab emerald ai system component from enemy
				if(enemies[i] != null){
					EmeraldAISystem EmeraldComponent = enemies[i].GetComponent<EmeraldAISystem>();
					if(EmeraldComponent.IsDead){
						// Remove enemy from list because hes now dead!
						enemies.Remove(enemies[i]);
						sm.coins = sm.coins +5; //increase money
						
					}
				}
			} // 

			// Check if the enemy list is empty
			if (enemies.Count == 0)
			{
				checkForDeaths();
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

				// Stop the coroutines
				StopCoroutine(checkListCoroutine);
			}
		}
	}


	public void stopCheckListCoroutine(){
		StopCoroutine(checkListCoroutine);

	
	}

	// Update is called once per frame
	void Update(){}


	void OnSceneLoaded( Scene scene, LoadSceneMode mode){
		if(scene.name == nightScene){
			//Activate all items with weapon tag
		
			//invoke event for loaded night scene
			onNightSceneLoaded.Invoke();
			
		       	//increment what level we are on.
			level++;			
			
			Debug.Log("Night loaded.");
			Debug.Log("Level #"+ level);

			//disable canvas
			StoreUI.GetComponent<Canvas>().enabled = false;

		

			// spawn monsters
			spawnMonsters();
			
			// switch sceneCode to 1
			sceneCode = NIGHT; 
		
		}else if(scene.name == dayScene || scene.name == "Tutorial"){		
			// Deactivate all items with weapon tag
			GameObject [] weapons = GameObject.FindGameObjectsWithTag("Weapon");
			foreach (GameObject w in weapons){
				w.SetActive(false);

				
			}

			// invoke event for loaded day scene
			onDaySceneLoaded.Invoke();
			Debug.Log("Day Loaded. ");
		
			//switch sceneCode to 0, 
			sceneCode= DAY ; 		
			
			//enable canvas
			StoreUI.GetComponent<Canvas>().enabled = true;
		} else if (scene.name == "end"){

				

		}


	}




	//if we go with GUI implementation this can be replaced
	// with a " on button / trigger pressed" type event
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && sceneCode == DAY){
			ChangeScene();
		}
	}

	public void ChangeScene()
	{
			//Unsubscribe from daytime sceneLoaded event
			SceneManager.sceneLoaded -= OnSceneLoaded; 

			// Invoke event to prepare for nighttime scene load
			beforeNightSceneLoaded.Invoke();

			// Load night scene
			SceneManager.LoadScene(nightScene);

			// Subscribe to nighttime sceneLoaded event
			SceneManager.sceneLoaded += OnSceneLoaded; 
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
			//	enemies.Add(ai);
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
				Debug.Log("Dead Animal Reported.");
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
	
	
	void spawnMonsters(){

		//  scavanger monster is at index 0
		//  bug is at index at index 1
			//	tanky monster is at index 2
		
		//This determines the max index from which a monster can be selected
		int max_index = 0;
		
		if (level > 2 ){
			max_index = 2;
		}

		// how many monsters to spawn into the world
		int howMany = level * 2; 

		// add two tanks
		if (level > 3){
			for(int i = 0 ; i < 2; i++){
			GameObject prefab = monsters[2];
			giveBirth(prefab);	
			}	
			howMany = howMany -2;
		}	

		

		// Loop through and spawn monsters
		for(int i = 0 ; i < howMany ; i ++){
			// Randomly select a monster prefab from the list
			int index = (int)(UnityEngine.Random.Range(0,max_index )); //integer division rounds down, so this excludes final monster
			GameObject prefab = monsters[index];
			giveBirth(prefab);

		}
	}
	
	void giveBirth(GameObject prefab){

			// Randomly select a number between a range
			// full unit circle  :  (0, 2*(float)Math.PI )

			// we do a partial circle
			float min = ((3*(float)Math.PI)/2);
			float max = (2*(float)Math.PI) + (((float)Math.PI)/2);
 			double theta = (double)UnityEngine.Random.Range(min,max);

			// Calculate cartesian coordinate
			float x = monsterSpawnRadius * (float)Math.Cos(theta);
			float z = monsterSpawnRadius * (float)Math.Sin(theta);
			float y = transform.position.y; 

			// Determine where monster will be spawned 
			Vector3 spawnCoordinate = transform.position  + new Vector3 (x,y,z);

			// instantiate monster into world 
			GameObject go = (GameObject)Instantiate(prefab, spawnCoordinate, Quaternion.identity);
			
			// Store refence to enemy 
			enemies.Add(go);

	}
	
	void OnDrawGizmos(){
		//Draw Animal spawn location sphere
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(spawnLoc.position, 0.5f);

		// Draw Monster spawn sphere
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, monsterSpawnRadius);
	}






}//end of class
