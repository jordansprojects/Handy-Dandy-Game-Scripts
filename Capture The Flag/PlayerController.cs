using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PlayerController : MonoBehaviour
{


	private Rigidbody2D rb;
	private Animator animator;
	[SerializeField] float moveSpeed = 5f;
	private SpriteRenderer spr;
	

	/* special ability variables */
	[SerializeField] Image movementSpeedAbility;
	[SerializeField] float speedIncrease = 3.0f;
	[SerializeField] float abilityTime = 10f; 
	[SerializeField] float abilityButtonCoolDown = 15f;
	float abilityTimer;
	float abilityCoolDownTimer;
	float speedMultiplier;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();      
		animator = GetComponent<Animator>();
		spr=GetComponent<SpriteRenderer>();
		abilityTimer = 0;
		abilityCoolDownTimer = abilityButtonCoolDown;
		speedMultiplier = 1.0f;


	}

	// Update is called once per frame
	void Update(){
		HandleKeyPress();
		UpdateTimers();
		if (Input.GetMouseButtonDown(0)){
			animator.SetTrigger("Shoot");
		}else{
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");
			Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
			rb.velocity = direction * moveSpeed * speedMultiplier;
			// Calculate the magnitude of movement to determine the animation speed
			float movementSpeed = rb.velocity.magnitude;
			animator.SetFloat("Speed", movementSpeed);
		}

		// clamp bar UI image w proper constraints
		movementSpeedAbility.fillAmount = Mathf.Clamp( abilityCoolDownTimer / abilityButtonCoolDown, 0 , 1);
	}


	void UpdateTimers(){
		if ( abilityTimer < abilityTime){
			abilityTimer+= Time.deltaTime;
		}else{
			speedMultiplier = 1.0f; //reset speedMultiplier 
		}
		if (abilityCoolDownTimer <= abilityButtonCoolDown){
			abilityCoolDownTimer +=Time.deltaTime;
		}
	}

	void HandleKeyPress(){
		/* if E key is pressed and the button is cooled down */
		if (Input.GetKey(KeyCode.E) && (abilityCoolDownTimer >= abilityButtonCoolDown)){
			Debug.Log("E pressed");
			speedMultiplier = speedIncrease;
			abilityCoolDownTimer = 0;
			abilityTimer =0;
		}
	}



}// end of PlayerController class
