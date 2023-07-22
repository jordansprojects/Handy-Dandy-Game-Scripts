using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    MeshRenderer mesh;
    float timer = 0;
    [SerializeField] float blinkOn = 1.0f;
    [SerializeField] float blinkOff = 0.5f;
    bool isOn = true;
    [SerializeField] float timeToDie = 30.0f;
    float totalTime = 0;
    // Start is called before the first frame update
    void Start()
    {
	    mesh = GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
	    timer += Time.deltaTime;
            totalTime+= Time.deltaTime;
	    Blink();

	    if(totalTime >= timeToDie){
	    	Destroy(gameObject);
	    }
    }


    void Blink(){
    	if ( isOn && timer > blinkOn){
		mesh.enabled = false;
		isOn = false;
		timer = 0;
	}else if(!isOn && timer > blinkOff){
		mesh.enabled = true;
		isOn = true;
		timer = 0;
	}
    }

}
