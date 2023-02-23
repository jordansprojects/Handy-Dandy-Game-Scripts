using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMonster : MonoBehaviour
{
    GameObject target;
    Vector3 location;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(move());

    }

    // Update is called once per frame
    void Update()
    {

    }

   IEnumerator move(){

        while(true){
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z+direction.z);
            yield return new WaitForSeconds(0.5f);
        }
 
   
    }
}
