using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float deathTimer = 0f;
    float timeToDie = 3f;
    // Start is called before the first frame update
    void Start()
    {
	    deathTimer = 0;
        
    }

    // Update is called once per frame
    void Update(){

	    // if not an orb projectile 
	    if (gameObject.tag == "Projectile"){
	   	Spin(); // rotate slightly every frame
	    }
	   /* keep track of time */ 
	    deathTimer+=Time.deltaTime;

	    /* destroy projectile after certain amount of time */
  	    if( deathTimer >= timeToDie){
	    	Destroy(gameObject);
	    }
    }


   // shift 1 degree
   // note : this method seems to cause some distortion
    private void Spin(){
    	transform.eulerAngles = transform.eulerAngles + new Vector3(0f,0f, 1f);
    }

   
}
