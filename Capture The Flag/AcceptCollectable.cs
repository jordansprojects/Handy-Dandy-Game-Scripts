using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AcceptCollectable : MonoBehaviour
{
     int spawnCount;
     [SerializeField] string spawnTag = "Heart Collectable";
     [SerializeField] string collectedTag = "Captured Heart";
     int acceptedCollectables;
     [SerializeField] UnityEvent allCollectedEvent;
     [SerializeField] UnityEvent collectedItemEvent;
     [SerializeField] AudioSource acceptedNoise;
     bool spawningFinished = false;

     ScoreTracker tracker;

    // Start is called before the first frame update
    void Start(){
	    // init accepted amount to 0
	    acceptedCollectables = 0;
            if ( GameObject.FindGameObjectWithTag("Score Tracker") != null){
	    	tracker = GameObject.FindGameObjectWithTag("Score Tracker").GetComponent<ScoreTracker>();
		tracker.startTracking();
	    }
    }

    public void SetSpawningFinishedToTrue(){
	    // count how many collectables there are in the scene
	    spawnCount = GameObject.FindGameObjectsWithTag(spawnTag).Length;
	    spawningFinished = true;
    	     if (tracker != null){
	    	tracker.spawnCount = spawnCount;
	     }
	    
    }

    // Update is called once per frame
    void FixedUpdate(){
	    // all collectables have been collected
	    if (spawningFinished){
		    if(spawnCount <= acceptedCollectables){
			    if ( tracker!= null) stopTracker();
			    allCollectedEvent.Invoke();
		    }
	    }

    }

/* seperate method is useful for event invocation*/
    public void stopTracker(){
    	if(tracker!= null){
		tracker.stopTracking();
	}
    }

    void OnTriggerEnter2D( Collider2D other){
    	if (other.gameObject.tag == collectedTag){
		collectedItemEvent.Invoke();
		if (acceptedNoise != null){
			acceptedNoise.Play();
		}
		// delete collected item
		Destroy(other.gameObject);
		// increment count
		acceptedCollectables++;
		if (tracker != null){
			tracker.collected++;
		}

	}
    }
}
