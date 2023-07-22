using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellCastController : MonoBehaviour
{

    Camera m_Camera;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float projectileSpeed = 10f;    
    private SpriteRenderer characterSpriteRenderer;
    [SerializeField] float delayBeforeShooting = 1f; // Adjust this value to set the delay in seconds
    Vector2 mousePosition;
    private Vector2 spawnPointLocalPosition ;
    


    /** cool down elements */
    [SerializeField] float maxCoolDown = 10.0f; //max cooldown in seconds
    float coolDownTimer = 0;
    [SerializeField] Image coolDownBar;


    /** Ability elements */
    [SerializeField] Image attackSpeedAbility;
     float attackSpeedButtonTimer;
    [SerializeField] float maxAttackSpeedButtonTime;
    [SerializeField] float maxAbilityEffectTime = 5.0f; 
    float attackSpeedAbilityInEffectTimer = 0;
    bool isAttackSpeedAbilityInEffect = false;

    /* shooting noises */
    [SerializeField] AudioSource emptyNoise;
    [SerializeField] AudioSource popNoise;
    // Start is called before the first frame update
    void Start(){
          m_Camera = Camera.main;
	  characterSpriteRenderer = GetComponent<SpriteRenderer>();
      	  spawnPointLocalPosition = spawnPoint.localPosition;
	
    
	  /* begin with cooled down with abilities enabled */
	  attackSpeedButtonTimer = maxAttackSpeedButtonTime;
   	  coolDownTimer = maxCoolDown;
   
    }


    // Update is called once per frame
    void Update(){
	    HandleTimers();
	    HandleMouseInput();
	    HandleKeyPress();
	    // clamp bar UI Image with proper constraints 
	    coolDownBar.fillAmount = Mathf.Clamp( coolDownTimer / maxCoolDown, 0, 1);
   	    attackSpeedAbility.fillAmount = Mathf.Clamp ( attackSpeedButtonTimer / maxAttackSpeedButtonTime, 0, 1);
    }

    void HandleTimers(){
    	if( coolDownTimer < maxCoolDown){
	    	coolDownTimer+=Time.deltaTime;
	    }
	    
	 if ( attackSpeedButtonTimer <= maxAttackSpeedButtonTime){
	    	attackSpeedButtonTimer+= Time.deltaTime;
	    }

	 if ( attackSpeedAbilityInEffectTimer >= maxAbilityEffectTime){
	 	isAttackSpeedAbilityInEffect = false;
		attackSpeedAbilityInEffectTimer = 0;
	 }else{
	 	attackSpeedAbilityInEffectTimer+=Time.deltaTime;
	 }
    }

    private void HandleKeyPress(){
	    // attack speed / cooldown reduction ability
	    if (Input.GetKey(KeyCode.Q) && (attackSpeedButtonTimer >= maxAttackSpeedButtonTime)){
		    
		    // put ability in effect
		    isAttackSpeedAbilityInEffect = true;
		    
		    coolDownTimer = maxCoolDown;

		    //reset button timer
		    attackSpeedButtonTimer = 0; 
	    }
    	
    }

    private void HandleMouseInput(){
	    // Flip the character sprite based on the direction of the mouse
	    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    if (mousePosition.x < transform.position.x)
	    {
		    characterSpriteRenderer.flipX = true; // Face left
		    spawnPoint.localPosition = new Vector3(-spawnPointLocalPosition.x, spawnPointLocalPosition.y);

	    }
	    else
	    {
		    characterSpriteRenderer.flipX = false; // Face right
		    spawnPoint.localPosition = spawnPointLocalPosition;

	    }
	    if (Input.GetMouseButtonDown(0)){
		    if (coolDownTimer >= maxCoolDown){
			    popNoise.Play();
			    Invoke("ShootProjectile", delayBeforeShooting);
			    // reset cooldown vars
			    coolDownTimer = 0;
	           // give coolDownTimer a boost if attack speed ability is in effect
			    if (isAttackSpeedAbilityInEffect ){
				    coolDownTimer = maxCoolDown * .75f ;
			    }
		    }else{
			    emptyNoise.Play();

		    }
	    }

    }


   private void ShootProjectile()
    {
        // Create a new projectile instance
        GameObject newProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();

        // Calculate the shooting direction based on the mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootingDirection = (mousePosition - (Vector2)spawnPoint.position).normalized;

        // Shoot the projectile in the calculated direction
        projectileRigidbody.velocity = shootingDirection * projectileSpeed;

        // Optional: Add any additional behavior or settings to the projectile here
        // For example, you could set a lifetime for the projectile or apply a force upon shooting.
    }

    

}// end of SpellCastController
