using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    // Reference to the AnimalStats script to get current stamina
    public AnimalStats animalStats;

    // Array of images representing each stamina segment
    public Image[] staminaSegments;

    // Number of segments
    private int numberOfSegments;

    void Start()
    {
        // Initialize the number of segments
        numberOfSegments = staminaSegments.Length;

        // Optional: If AnimalStats is not assigned in the inspector, try to find it
        if (animalStats == null)
        {
            animalStats = GetComponent<AnimalStats>();
        }
    }

    void Update()
    {
        // Update the stamina bar every frame based on current stamina
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        // Calculate the percentage of stamina per segment
        float staminaPerSegment = animalStats.maxEnergy / (float)numberOfSegments;

        // Loop through each segment and update it from right to left
        for (int i = 0; i < numberOfSegments; i++)
        {
            // Calculate the threshold for each segment (each segment represents a fraction of the total stamina)
            float segmentThreshold = (i + 1) * staminaPerSegment;

            // Update the fill amount based on the current stamina
            if (animalStats.currentEnergy >= segmentThreshold)
            {
                // Full segment
                staminaSegments[i].fillAmount = 1f; // Fully filled
            }
            else if (animalStats.currentEnergy > (i * staminaPerSegment))
            {
                // Partially filled segment
                staminaSegments[i].fillAmount = (animalStats.currentEnergy - (i * staminaPerSegment)) / staminaPerSegment;
            }
            else
            {
                // Empty segment
                staminaSegments[i].fillAmount = 0f;
            }
        }
    }
}