using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInfo : MonoBehaviour
{
    TextMeshProUGUI textObj;
    ScoreTracker tracker;
    float scoreTime = 0;
    // Start is called before the first frame update
    void Start()
    {
	    GameObject scoreTrackerObj = GameObject.FindGameObjectWithTag("Score Tracker");
	    if (scoreTrackerObj != null){
	    	tracker = scoreTrackerObj.GetComponent<ScoreTracker>();
	    }
	    textObj = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update(){
        scoreTime+= Time.deltaTime;
    }

    void FixedUpdate(){
    	changeInfo();
		if(tracker!= null){
			if (scoreTime > 2.0f){
				tracker.calculateScore();
				scoreTime=0;
			}
		}
	}

	

    void changeInfo(){
	    if( tracker != null && textObj != null){
	    	string outputString = "Score : " + tracker.score + "\nKills : "+tracker.kills + "\nHearts : " +tracker.collected +"/" + tracker.spawnCount + "\nTime : " + tracker.timer.ToString("F2");
	    	textObj.text = outputString; 
	    }else{
	    	textObj.text = "ScoreTracker or TextMeshProUGUI is null\n";
	    }


    
    }

}
