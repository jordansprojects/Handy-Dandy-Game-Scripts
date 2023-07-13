using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    // to lift weapons 1 unit into the air, gravity will pull them down
    Vector3 displacementY = new Vector3(0, 0.5f, 0);
    // spawn points
    public List<Transform> spawnPoints = new List<Transform>();

    // weapon prefabs     
    public List<GameObject> weapons = new List<GameObject>();

    // Spotlight prefab
    public GameObject spotlightPrefab;

    // Spotlight offset
    Vector3 spotlightOffset = new Vector3(0, 3f, 0); // adjust as needed


    public void spawnWeapons()
    {
        // loop through spawn points
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            // randomly select a weapon
            int index = (int)(UnityEngine.Random.Range(0, weapons.Count));
            GameObject prefab = weapons[index];
            GameObject weaponInstance = Instantiate(prefab, (spawnPoints[i].position + displacementY), Quaternion.identity);

            // instantiate the spotlight above the weapon
            GameObject spotlightInstance = Instantiate(spotlightPrefab, (spawnPoints[i].position + spotlightOffset), Quaternion.Euler(90, 0, 0));
        }
    }
} // end of class 
