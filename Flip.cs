using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Flip : MonoBehaviour
{
    public bool printDebugInfo = true;
    bool hitOnce = false;
  
	
    float jumpHeight = 3.0f;


   // Start is called before the first frame update
    void Start()
    
    {
    }

    void Update(){
	    if(hitOnce == true){
	     
	    }
    
    
    }
    // This function is called when another collider hits this object's collider
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player" && !hitOnce)
        {
            StartCoroutine(jumpAndFlip());
            string myname = gameObject.name;
            if (printDebugInfo)
                Debug.Log("Player hits me. My name is: " + myname);             
	    hitOnce = true;
        }
    }

    IEnumerator jumpAndFlip(){
            // lift  him in the air so it seems like hes jumping
           gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y+ jumpHeight,transform.localPosition.z);
           yield return new WaitForSeconds(0.15f);
            //rotate
            for (int i = 0; i <4 ; i++ ){
	            gameObject.transform.Rotate(new Vector3(-90,0,0), Space.World);
                yield return new WaitForSeconds(0.05f);
            }

	        //wait
            yield return new WaitForSeconds(0.15f);
            // put him back down
            gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y- jumpHeight,transform.localPosition.z);
            hitOnce = false; //set hitonce back to false since he has landed. 

    }
}