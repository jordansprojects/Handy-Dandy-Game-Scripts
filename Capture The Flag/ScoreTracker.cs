using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ScoreTracker : MonoBehaviour
{
	static ScoreTracker instance;	
	public int kills  = 0;
	public int collected = 0;
	public int spawnCount = 0;
	public float timer  = 0;
	public int highestScore = 0;
	public int score = 0; 
	bool gameIsOn = false;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update(){
		if (gameIsOn){
			timer += Time.deltaTime;
		}
	}

	public void updateScoreWithKill(){
		kills++;
	}


	public void resetTimer(){
		timer = 0;

		/* putting this here temporarily bc im lazy */
		resetValues();
	}

	public void resetValues(){
		highestScore = (score > highestScore)? score : highestScore;
		kills = collected = score = 0;
	
	}


	public float calculateScore(){
		if (gameIsOn){
			score+= (int) ((timer*100 > 0 ) ? ( (kills + collected) * (1/timer*100)) : 0 );	
		}
		return score;
	}

	public void startTracking(){
		gameIsOn = true;
		resetTimer();
	}

	public void stopTracking(){
		gameIsOn = false;
		/* calculate time bonus */
		score+= (int)((score * (1/timer)) );// it should not be possible for timer to ever be zero. 
	}

	void Awake(){ 
		DontDestroyOnLoad (this);		
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}


}
