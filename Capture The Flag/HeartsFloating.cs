using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsFloating : MonoBehaviour
{
    [SerializeField] Transform origin; 
    [SerializeField] GameObject heartPrefab; // Reference to the heart prefab
    [SerializeField] float radius = 1.5f; // Radius of the circular motion
    [SerializeField] float speed = 1.5f; // Speed of the circular motion
   [SerializeField] AudioSource collectedNoise;
   [SerializeField] string collectableTag = "Heart Collectable";
   [SerializeField] int maxHearts = 7;
    private List<Transform> hearts = new List<Transform>();

    void Start(){
    }

    void Update()
    {
        // Update the position of the hearts in a circular motion around the character's head
        for (int i = 0; i < hearts.Count ; i++)
        {
	    if (hearts[i] == null){
		Transform itemToRemove = hearts[i];
		hearts.RemoveAt(i);
		continue;
	    }
            float angle = Time.time * speed + i * 360f / maxHearts;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
            hearts[i].position = origin.position + offset;
        }
    }


    void OnTriggerEnter2D(Collider2D other){
	    if (other.gameObject.tag == collectableTag){
		    collectedNoise.Play();
		    Destroy(other.gameObject);

		    float angle = hearts.Count * 360f / maxHearts;
		    Vector3 position = origin.position + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;


		    hearts.Add(Instantiate(heartPrefab, position, Quaternion.identity).transform);
	    }
    }



}

