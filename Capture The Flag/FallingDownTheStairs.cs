using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** reference source: https://forum.unity.com/threads/how-to-make-gameobject-gradually-decrease-in-size.1033645/ */
public class FallingDownTheStairs : MonoBehaviour
{
	[SerializeField] AudioSource fallSound;
	[SerializeField] Health playerHealth;
	[SerializeField] GameObject spriteToShrink;
	static bool fellOnce = false;
	[SerializeField] Image blackScreen;


	void Start(){
		// set color of the panel transparent
		blackScreen.color = new Color(0,0,0,0);
		fellOnce = false;
	}	

	void GoBlack(){
		// set color of the panel - black
		blackScreen.color = new Color(0,0,0,255);
	}
	
	
	void OnTriggerEnter2D(Collider2D other ){
		if ( other.gameObject.tag == "Player" && fellOnce == false){
			GoBlack();
			fallSound.Play();
			playerHealth.TakeDamage((int)playerHealth.health);
			fellOnce = true;

		}
	}


}
