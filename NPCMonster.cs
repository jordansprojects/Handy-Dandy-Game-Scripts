using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMonster : MonoBehaviour
{
    GameObject target;
    AudioSource collideAudio, explodeAudio;
    public GameObject explosionEffect;
    Vector3 location;
    public GameObject explosionrubble;
    public bool disableWhenHit = true;

    ParticleSystem ps;
 
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera");
        collideAudio = GameObject.Find("CollideSound").GetComponent<AudioSource>();
        explodeAudio = GameObject.Find("ExplodeSound").GetComponent<AudioSource>();
        ps = gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(move());
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }

 void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name.Contains("Spell"))
        {
            Debug.Log("hit target");
            GameObject effect = Instantiate(explosionEffect,gameObject.transform.position  , Quaternion.identity );
           // ps.Play(); //dis not workin
            explodeAudio.Play(); //sound when character hits user
            //enable the explosion particle effect
            explosionrubble.SetActive(true); //dis not workin either
            if(disableWhenHit)
                //disable the target wall
                gameObject.SetActive(false);
                Destroy(gameObject,3.0f);
            
            
        }
    }

    // coroutine to moveobject
   IEnumerator move(){
        while(true){
            Vector3 direction = ((target.transform.position - transform.position).normalized)/4;
            transform.position = new Vector3(transform.position.x+ direction.x, transform.position.y, transform.position.z+direction.z);
            yield return new WaitForSeconds(0.07f);
        }
    }


    void OnTriggerEnter(Collider other){
        //this is janky asf
        if(other.gameObject.name.Contains("Chair") || other.gameObject.name.Contains("VRCamera") || other.gameObject.name.Contains("Player" )
        || other.gameObject.tag == "Chair" || other.gameObject.tag == "Player" || other.gameObject.tag == "VRCamera"){
            Debug.Log("Player Hit by Character!");
            collideAudio.Play(); //sound when character hits user
            triggerDeath();
        }
    }

    //plays a sound and destroy self
    void triggerDeath(){
        Destroy(gameObject,1.0f);
        
    }
}// end of class



