using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSimple : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private static LoadSceneSimple instance;
    [SerializeField] private float delayTime = 1.0f; 
   // Start is called before the first frame update
    void Start(){
	
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   void OnSceneLoaded( Scene scene, LoadSceneMode mode){
   }
 

  IEnumerator DelayBeforeLoad(){
  	yield return new WaitForSeconds(delayTime);
	OpenScene();
  } 

  public void OpenSceneWaitWithCoroutine(){
  	StartCoroutine(DelayBeforeLoad());
  }

   public void OpenScene(){
	// unsubscribe to current scene
	SceneManager.sceneLoaded -= OnSceneLoaded;
	
	// load scene to transistion to
	SceneManager.LoadScene(sceneName);

	// subscribe to current scene
	SceneManager.sceneLoaded+= OnSceneLoaded;
    }


}
