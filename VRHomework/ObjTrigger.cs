using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTrigger : MonoBehaviour
{
    public bool printDebugInfo = true;
    bool hitOnce = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // This function is called when another collider hits this object's collider
    void OnTriggerEnter(Collider other)
    {
	float scale = 5f;
        if (other.name == "Player" && !hitOnce)
        {
            string myname = gameObject.name;
            if (printDebugInfo)
                Debug.Log("Player hits me. My name is: " + myname);

            //your code here. Let the object do something when it is hit by the player
	        transform.localScale = new Vector3(transform.localScale.x*scale,transform.localScale.y*scale,transform.localScale.z*scale);
           //line below was in Jeffs but i did not find it necessary for mine
           // transform.position = transform.position + new Vector3(4,0,3);           
            hitOnce = false; 
        }
    }
}
