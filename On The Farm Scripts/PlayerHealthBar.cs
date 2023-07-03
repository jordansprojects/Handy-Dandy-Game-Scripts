using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI.Example;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject[] Hearts; // Array of heart gameobjects, assign in inspector
    private EmeraldAIPlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<EmeraldAIPlayerHealth>();
        if(playerHealth == null)
        {
            Debug.LogError("EmeraldAIPlayerHealth component not found on the player.");
        }
    }

    private void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        if (playerHealth != null)
        {
            for (int i = 0; i < Hearts.Length; i++)
            {
                if (i < playerHealth.CurrentHealth / 20)
                {
                    Hearts[i].SetActive(true); // Enable heart if health is above 20*(i+1)
                }
                else
                {
                    Hearts[i].SetActive(false); // Disable heart if health is below 20*(i+1)
                }
            }
        }
    }
}
