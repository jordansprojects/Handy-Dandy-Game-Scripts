using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float deathTimer = 0f;
    float timeToDie = 3f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
	    deathTimer = 0;
	    rb = GetComponent<Rigidbody2D>();
        
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

	    float velocityMagnitude = rb.velocity.sqrMagnitude;
	    velocityMagnitude  = (float)System.Math.Round(velocityMagnitude, 2);
	    if  ( velocityMagnitude < 0.5f){
	    	Destroy(gameObject);
	    }
    }


   // shift 1 degree
   // note : this method seems to cause some distortion
    private void Spin(){
    	transform.eulerAngles = transform.eulerAngles + new Vector3(0f,0f, 1f);
    }

   
}
