using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNoise : MonoBehaviour
{
   private static PlayNoise instance;
    // Start is called before the first frame update
    void Start(){
    
    }

    // Update is called once per frame
    void Update()
    {
        
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