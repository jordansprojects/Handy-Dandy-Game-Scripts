using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using EmeraldAI;
using System.Linq; // for ToList() method
public class ShopManager : MonoBehaviour
{
  public int coins;
  public int moneyChange = 20;
  public TMP_Text coinsUI;
  public ShopItemSO[] shopItemsSO;
  public ShopTemplate[] shopPanels;
  public GameObject[] shopPanelsGO;
  public Button[] purchaseButton;

  // Reference to Game Daemon.
  public GameDaemon daemon;

  private GameObject[] animalPrefabs;

  private List<GameObject> totalAIs;
  private List<GameObject> enemies;
  private List<GameObject> allies;
  
  
  // Start is called before the first frame update
  void Start()
  {
	// init enemies List
	enemies =  new List<GameObject>();
  // init allies List
  allies = new List<GameObject>();

    coins = 50;
    animalPrefabs = daemon.dayAnimals; //Object ref not set error here
    for(int i = 0; i < shopItemsSO.Length; i++){
      shopPanelsGO[i].SetActive(true);
    }
    coinsUI.text = "Coins: " + coins.ToString();
    loadPanels();
    CheckPurchasable();
  }

  // Update is called once per frame
  void Update()
  {


	  // changed this from for loop to for-each loop due to enumeration error
/*	  for ( int i = 0; i < enemies.Count; i++){
		  EmeraldAISystem EmeraldComponent = enemies[i].GetComponent<EmeraldAISystem>();
		  if(EmeraldComponent.IsDead){
			  coins = coins + moneyChange;
		//	  Debug.Log(coins);
			  // Remove enemy from list because hes now dead!
			  enemies.Remove(enemies[i]);
        // Updates coin amount in UI
        coinsUI.text = "Coins: " + coins.ToString();
		  }


	  } */
  }


  public void updateCoins(){
	Debug.Log("updateCoins invoked");
	// Increase coin amount
  	coins= coins + moneyChange;

	// Updates coin amount in UI
        coinsUI.text = "Coins: " + coins.ToString();

  }


  public void loadPanels(){

    for(int i = 0; i < shopItemsSO.Length; i++){
      shopPanels[i].titleTxt.text = shopItemsSO[i].title;
      shopPanels[i].descTxt.text = shopItemsSO[i].desc;
      shopPanels[i].costTxt.text = "Coins: " + shopItemsSO[i].cost.ToString();
    }
  }

  public void CheckPurchasable(){

    for(int i = 0; i < shopItemsSO.Length; i++){
      if(coins >= shopItemsSO[i].cost){
        purchaseButton[i].interactable = true;
      }else{
        purchaseButton[i].interactable = false;
      }
    }
  }

/***************************************************************************
 * This is responsible for counting all instances of enemies in the scene
 * It is invoked in an event by the 'MagicBed' SceneTransistion script 
 **************************************************************************/
public void populateEnemyArray(){
	Debug.Log("Invoking populateEnemyArray so store all instances of enemy.");
    	// collects all objects with this tag
    	totalAIs = GameObject.FindGameObjectsWithTag("AI").ToList();
    
	
    foreach (GameObject ai in totalAIs){
          EmeraldAISystem sys = ai.GetComponent<EmeraldAISystem>();
          //Debug.Log("faction # : " + sys.CurrentFaction);
          if ( sys.CurrentFaction == 0){
              enemies.Add(ai);
          }else{
              allies.Add(ai);
          } 
    } // end of loop
        Debug.Log("Number of enemies in this scene : " + enemies.Count);
        Debug.Log("Number of allies in this scene : " + allies.Count);
}


/*
  Number of buttons should not be more than the number of items we have

*/
  public void purchaseItem(int btnNum){
    if(coins >= shopItemsSO[btnNum].cost){
      coins = coins - shopItemsSO[btnNum].cost;
      coinsUI.text = "Coins: " + coins.ToString();
      CheckPurchasable();

      // This is a temporary implementation that will need to be changed.
      daemon.CreateAIObject(animalPrefabs[btnNum], btnNum)      ;
    }
  }
}
