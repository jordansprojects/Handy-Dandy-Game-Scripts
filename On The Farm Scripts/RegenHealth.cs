using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI.CharacterController;
using EmeraldAI.Example;
public class RegenHealth : MonoBehaviour
{
    EmeraldAIPlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
	    health = GetComponent<EmeraldAIPlayerHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // regen function is invoked when player respawns in day mode 
    public void regen(){
	// set health back to 100 on regen
   	 health.CurrentHealth =100;
    }
}
