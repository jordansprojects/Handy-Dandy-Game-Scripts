using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //VR player data
    public GameObject player;
    // Array of enemy prefabs 
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    public bool [] isOccupied; 
    public float spawnTime = 5.0f;
    // Indexing variables
    int index, maxIndex, numLocations; 
    
    // Start is called before the first frame update
    void Start()
    { 
        // grab number of prefabs in the list  
        maxIndex = enemies.Length;
        numLocations = spawnPoints.Length;

        
    }


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(spawn());
    }


    // Enemy generator Coroutine function
    // @param GameObject obj : Object to be instantiated 
     IEnumerator spawn(){

        while(true){
         //generate random index to choose prefab from prefab array
        index = Random.Range(0, maxIndex);
        GameObject obj = enemies[index];
        //generate random index to choose the spawn location from spawn object location array
        int spawnIndex = Random.Range(0,numLocations);
        GameObject spawnObj = spawnPoints[spawnIndex]; 
        // indicate location is taken - this may not be necessary! 
        isOccupied[spawnIndex] = true;
        // Instanitate example (delete later)
        //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        GameObject enemy = Instantiate(obj,spawnObj.transform.position  , Quaternion.identity );
        // ideal transform coordinates -0.4f,-195.0f,1.0f
        yield return new WaitForSeconds(spawnTime);

        }
    }





}
