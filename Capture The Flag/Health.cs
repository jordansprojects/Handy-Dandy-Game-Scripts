using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Handles character health (player and AI) and damage taking interactions */


public class Health : MonoBehaviour
{


	/* health variables */
	float health;
	float maxHealth = 100;
	[SerializeField] Image healthBar;


	/* sprite blinker variables */
	private float spriteBlinkingTimer = 0.0f;
	private float spriteBlinkingMiniDuration = 0.1f;
	private float spriteBlinkingTotalTimer = 0.0f;
	private float spriteBlinkingTotalDuration = 1.0f;
	private bool startBlinking = false;
	SpriteRenderer spr;
	[SerializeField] Color hurtColor;
	

	/* hit by projectile variables */
	 SpriteRenderer projectileSR;
	/* projectile having its own audio is useful if you want custom
	   sounds depending upon the weapon. For now, we are relying only
	   on impactNoise
	   private AudioSource projectileAudio; */

	/* customizeable fields for different character interactions and experiences
	 * such as a sound of being hit by something or a character crying out in pain */
	[SerializeField] string projectileTag = "Projectile";
	[SerializeField] AudioSource impactNoise; 
	[SerializeField] AudioSource yelpNoise;
	[SerializeField] float soundDelay  = 0;

	// Start is called before the first frame update
	void Start(){
		spr=GetComponent<SpriteRenderer>();
		health = maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		if(startBlinking == true){ /* check for blinking sprite */
		        changeColor(); 	
			SpriteBlinkingEffect();
		}
		// clamp bar UI to proper constraints
		healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
	}




	private void changeColor(){
		if (startBlinking  == true){
			spr.color = hurtColor;
		}else{
			spr.color = Color.white;
		}
	}

	private void OnTriggerEnter2D(Collider2D col){
			
		if(col.gameObject.tag == projectileTag ){
			//projectileAudio = col.gameObject.GetComponent<AudioSource>();
			projectileSR = col.gameObject.GetComponent<SpriteRenderer>(); 

			startBlinking = true;
		

			if (impactNoise != null){
				impactNoise.time = soundDelay;
				impactNoise.Play();
			}
			if( health < 30){
				if (yelpNoise != null){
					yelpNoise.Play();
				}
			}

			projectileSR.enabled = false;
			// destroy obj
			Destroy(col.gameObject, 1f);
			TakeDamage(10);
		}

	

	}




	/* Blinks sprite for predefined period of time . Taken from this article: https://discussions.unity.com/t/sprite-blinking-effect-when-player-hit/158164/2 */
	private void SpriteBlinkingEffect()
	{
		spriteBlinkingTotalTimer += Time.deltaTime;
		if(spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
		{
			startBlinking = false;
			spriteBlinkingTotalTimer = 0.0f;
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;   // according to 
											  //your sprite
			changeColor(); /* change color back when blinking is finished */
			return;
		}

		spriteBlinkingTimer += Time.deltaTime;
		if(spriteBlinkingTimer >= spriteBlinkingMiniDuration)
		{
			spriteBlinkingTimer = 0.0f;
			if (this.gameObject.GetComponent<SpriteRenderer> ().enabled == true) {
				this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;  //make changes
			} else {
				this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;   //make changes
			}
		}
	}


	private void TakeDamage(int dmg){
		health-=dmg;
	}


} // end of Health class
