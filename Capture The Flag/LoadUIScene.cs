using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUIScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    AudioSource audioSrc;
    private static LoadUIScene instance; 
   // Start is called before the first frame update
    void Start(){
	audioSrc = GameObject.FindGameObjectWithTag("SoundMaker").GetComponent<AudioSource>();
	
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   void OnSceneLoaded( Scene scene, LoadSceneMode mode){
	// Play sound
	StartCoroutine(playButtonSound());
   }
   
   IEnumerator playButtonSound(){
	audioSrc.Play();
	yield return new WaitWhile (()=> audioSrc.isPlaying);
	//do something
   }

   public void OpenScene(){
	// unsubscribe to current scene
	SceneManager.sceneLoaded -= OnSceneLoaded;
	
	// load scene to transistion to
	SceneManager.LoadScene(sceneName);

	// subscribe to current scene
	SceneManager.sceneLoaded+= OnSceneLoaded;
    }

  void Awake(){ 
	DontDestroyOnLoad (this);		
	if (instance == null) {
		instance = this;
	} else {
		Destroy(gameObject);
	}
}

}
