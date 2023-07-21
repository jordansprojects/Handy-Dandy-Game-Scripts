using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerController : MonoBehaviour
{


	[SerializeField] float animationSpeedMultiplier = 1f;
	private Rigidbody2D rb;
	private Animator animator;
	[SerializeField] float moveSpeed = 5f;
	private SpriteRenderer spr;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();      
		animator = GetComponent<Animator>();
		spr=GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update(){
		if (Input.GetMouseButtonDown(0)){
			animator.SetTrigger("Shoot");
		}else{
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");

			Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
			rb.velocity = direction * moveSpeed;
			// Calculate the magnitude of movement to determine the animation speed
			float movementSpeed = rb.velocity.magnitude;
			animator.SetFloat("Speed", movementSpeed * animationSpeedMultiplier);
		}
	}



}// end of PlayerController class
