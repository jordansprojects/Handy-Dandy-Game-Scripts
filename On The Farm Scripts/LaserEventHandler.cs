using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/***********
 * Script so that a guns laser is only activated on grab
 */

public class LaserEventHandler : MonoBehaviour
{
    public GameObject laser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void activateLaser(){
    	laser.SetActive(true);
    }

    public void deactivateLaser(){
    	laser.SetActive(false);
    }

}
