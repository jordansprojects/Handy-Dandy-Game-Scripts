using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JumpRope : MonoBehaviour
{
    public GameObject textMeshObject;
    TextMeshPro textMesh;
    public float RotationSpeed = 5.0f;

    string checkPointTag= "Respawn";
    // Number of times banana rotates 
    int rotations = 0;
    Quaternion startingRot;

    // Start is called before the first frame update
    void Start(){
	   textMesh = textMeshObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update(){
	


    }

    void FixedUpdate(){
	    transform.Rotate( Vector3.right * ( RotationSpeed * Time.deltaTime ) * -1 );
    }

    private void OnTriggerEnter(Collider other){
    	
	    if ( other.gameObject.tag == checkPointTag){
		   // Increment Rotation Count
		    rotations++;
		    
		    // Increment Rotation Speed
		    RotationSpeed+=10;

		    // Display Text to UI
		    textMesh.text = "POINTS : " + rotations;
	}
    	
    }
}
