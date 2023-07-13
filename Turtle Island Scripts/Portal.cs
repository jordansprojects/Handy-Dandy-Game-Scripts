using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){

/* Disabled rotation Since the Turtle can see the world more easily now */
/*    	if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            direction.y = 0f; // Lock rotation only on the horizontal plane

            // Rotate the object to look at the target
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            }
        }*/
    }

   void OnSceneLoaded( Scene scene, LoadSceneMode mode){
   
   }

    void OnTriggerEnter(Collider other){
    
    	if (other.gameObject.tag == "turtle"){
		// unsubscribe from current scene 
		SceneManager.sceneLoaded -= OnSceneLoaded;
		
		// load scene to transistion to 
		SceneManager.LoadScene(sceneName);

		//subscribe to current scene
		SceneManager.sceneLoaded+= OnSceneLoaded;
	}
    }
}
