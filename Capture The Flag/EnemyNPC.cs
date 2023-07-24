using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyNPC : MonoBehaviour
{

	/* shoot projectile variables */
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform spawnPoint;
	[SerializeField] float projectileSpeed;
	[SerializeField] private float shootTimer = 0;
	[SerializeField] private float shootMaxTime;
	[SerializeField] private float shootMinTime;
	[SerializeField] private float shootInterval;
	[SerializeField] private AudioSource shootSoundEffect;



	/* death variables */
	[SerializeField] AudioSource deathSound;
	[SerializeField] float soundStart;
	[SerializeField] float soundEnd;

	/* movement and animation variables */
	private Animator animator;
	[SerializeField] float moveSpeed = 5f;
	private SpriteRenderer spr;
	private Transform target;
	private Rigidbody2D rb;
	[SerializeField] float minDistance = 2; // The desired distance from the player
	private GameObject [] NPCs;

	[SerializeField] List<Transform> wayPointLimits;

	[SerializeField] float avoidFactor ; // Boids algorithm separation variable to keep enemies from being too close
	private Vector2 spawnPointLocalPosition;

	/* convex hull navigation variables */
	[SerializeField] bool usesConvexHullNavigation = false;	
	[SerializeField] GrahamScanner grahamScanner;
	List<Vector2> convexHull;
	[SerializeField] float wayPointStopDistance = 0.1f; //may want this smaller or larger depending on the size of the convex hull
	Vector2 wayPoint;

	// Start is called before the first frame update
	void Start(){
		animator = GetComponent<Animator>();
		spr=GetComponent<SpriteRenderer>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
		NPCs = GameObject.FindGameObjectsWithTag("AI");
		spawnPointLocalPosition = spawnPoint.localPosition;
		shootTimer = 0;

		if (usesConvexHullNavigation){
			// calculate generate convex hull
			convexHull = grahamScanner.GrahamScan(wayPointLimits);
			GenerateWayPoint(); // set initial waypoint
		}

		//init the first shoot interval
		shootInterval = Random.Range(shootMinTime, shootMaxTime);

	}

	// Update is called once per frame
	void Update(){
		shootTimer+= Time.deltaTime;
		if (shootTimer > shootInterval){
			ShootProjectile();
		}	
		
	}

	void FixedUpdate(){
		if(!usesConvexHullNavigation){
			FollowPlayerTarget();
		}else{
			FollowGeneratedWayPoints();
		}
		SpreadEnemies();
	}

	public void Death(){
		// prepare to play funny sound
		deathSound.time = soundStart;
		deathSound.Play();
		deathSound.SetScheduledEndTime(AudioSettings.dspTime+(soundEnd - soundStart));
		// destroy object
		Destroy(gameObject);
	}

	private void FollowPlayerTarget(){
		// Determine direction to travel to based on target position
		Vector2 direction = (target.position - transform.position).normalized;
		if (Vector2.Distance(target.position, transform.position) > minDistance){
			// move if not too close
			rb.velocity = direction * moveSpeed *Time.deltaTime;
		}else{
			// dont move, and shoot at player
			rb.velocity = Vector2.zero;

		}

		WatchPlayer();	
	}


	public void WatchPlayer(){
	// Flip the sprite based on relative position to target
		if (transform.position.x  > target.position.x)
		{
			// Facing left
			spr.flipX = true;
			spawnPoint.localPosition = new Vector3(-spawnPointLocalPosition.x, spawnPointLocalPosition.y);
		}
		else if (transform.position.x < target.position.x)
		{
			// Facing right
			spr.flipX = false;
			spawnPoint.localPosition = spawnPointLocalPosition;

		}
	
	}

	public void FollowGeneratedWayPoints(){
		if ( Vector2.Distance(transform.position, wayPoint ) <= wayPointStopDistance){
			GenerateWayPoint();
		}else{
			Vector2 direction = (wayPoint - (Vector2)transform.position).normalized;
			rb.velocity = direction * moveSpeed *Time.deltaTime;
		}
		WatchPlayer(); // stay turned towards the player regardless

	}

	void GenerateWayPoint(){
		// Calculate the bounding box of the convex hull
		Vector2 minPoint = convexHull[0];
		Vector2 maxPoint = convexHull[0];
		foreach (Vector2 point in convexHull)
		{
			minPoint = Vector2.Min(minPoint, point);
			maxPoint = Vector2.Max(maxPoint, point);
		}

		Vector2 randomPoint = new Vector2(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y));
		while (!grahamScanner.IsPointInConvexHull(randomPoint, convexHull))
		{
			randomPoint = new Vector2(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y));
		}


		wayPoint = randomPoint;



	}

	private void ShootProjectile(){
		shootSoundEffect.Play();
		// Create a new projectile instance
		GameObject newProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
		// Get the Rigidbody2D component of the projectile
		Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();

		// Calculate the shooting direction based on the target position
		Vector2 targetPosition = target.position;
		Vector2 shootingDirection = (targetPosition - (Vector2)spawnPoint.position).normalized;

		// Shoot the projectile in the calculated direction
		projectileRigidbody.velocity = shootingDirection * projectileSpeed;

		animator.SetTrigger("Shoot");
		
		//reset  shoot variables
		shootInterval = Random.Range(shootMinTime, shootMaxTime);
		shootTimer = 0;

	}

	/* Keep enemies from getting too close to eachother
	 * Naive approach, modifies y value so its not too close */
	private void SpreadEnemies()
	{
		float close_dx = 0;
		float close_dy = 0;
		foreach (GameObject npc in NPCs){
			if (npc != null){
			close_dx += transform.position.x -  npc.transform.position.x;
			close_dy += transform.position.y - npc.transform.position.y;
			}
		}
		// update velocity
		Vector2 prevVelocity = rb.velocity;
		rb.velocity = new Vector2 (prevVelocity.x += (close_dx * avoidFactor) , prevVelocity.y +=  (close_dy *avoidFactor));	


	}


}// end of Enemy NPC class

