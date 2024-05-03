using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public FinalBossHealth finalbossHealth; // Reference to the boss health script
    public FinalBossPhase1 phase1Script; // Reference to the phase 1 script
    public FinalBoss finalBoss; // Reference to the final boss script
    public Rigidbody bossRigidbody; // Reference to the boss's Rigidbody component
    public int healthThreshold = 5; // Health threshold to trigger script enabling/disabling

    private bool hasTriggered = false; // Flag to ensure actions are performed only once

    void Update()
    {
        // Check if the boss's health is below the threshold and the actions haven't been triggered yet
        if (finalbossHealth.currentHealth <= healthThreshold && !hasTriggered)
        {
            // Disable phase 1 script and enable final boss script
            if (phase1Script != null)
            {
                phase1Script.enabled = false;
            }

            if (finalBoss != null)
            {
                finalBoss.enabled = true;
            }

            // Add Rigidbody component to the boss
            if (bossRigidbody != null)
            {
                bossRigidbody.isKinematic = false;
            }

            // Set the flag to indicate that actions have been triggered
            hasTriggered = true;
        }
    }
}