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
	float scale = 5.0f;
        if (other.name == "Player" && !hitOnce)
        {
            string myname = gameObject.name;
            if (printDebugInfo)
                Debug.Log("Player hits me. My name is: " + myname);

            //your code here. Let the object do something when it is hit by the player
	    gameObject.transform.localScale = new Vector3(scale,scale,scale);
            hitOnce = false;
        }
    }
}
