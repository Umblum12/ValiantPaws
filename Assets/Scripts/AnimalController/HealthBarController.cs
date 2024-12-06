using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Reference to the AnimalStats script to get current health
    public AnimalStats animalStats;

    // Array of images representing each health segment (Heart1, Heart2, Heart3)
    public Image[] healthSegments;

    // Number of segments
    private int numberOfSegments;

    void Start()
    {
        // Initialize the number of segments
        numberOfSegments = healthSegments.Length;

        // Optional: If AnimalStats is not assigned in the inspector, try to find it
        if (animalStats == null)
        {
            animalStats = GetComponent<AnimalStats>();
        }
    }

    void Update()
    {
        // Update the health bar every frame based on current health
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // Calculate the percentage of health per segment
        float healthPerSegment = animalStats.maxHealth / (float)numberOfSegments;

        // Loop through each segment and update it from right to left
        for (int i = 0; i < numberOfSegments; i++)
        {
            // Calculate the threshold for each segment (each segment represents a fraction of the total health)
            float segmentThreshold = (i + 1) * healthPerSegment;

            // Update the fill amount based on the current health
            if (animalStats.currentHealth >= segmentThreshold)
            {
                // Full segment
                healthSegments[i].fillAmount = 1f; // Fully filled
            }
            else if (animalStats.currentHealth > (i * healthPerSegment))
            {
                // Partially filled segment
                healthSegments[i].fillAmount = (animalStats.currentHealth - (i * healthPerSegment)) / healthPerSegment;
            }
            else
            {
                // Empty segment
                healthSegments[i].fillAmount = 0f;
            }
        }
    }
}