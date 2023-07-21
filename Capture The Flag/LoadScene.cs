using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    AudioSource audioSrc;
 
   // Start is called before the first frame update
    void Start()
    {
	audioSrc = GameObject.FindGameObjectWithTag("SoundMaker").GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   void OnSceneLoaded( Scene scene, LoadSceneMode mode){
	audioSrc.Play();
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
