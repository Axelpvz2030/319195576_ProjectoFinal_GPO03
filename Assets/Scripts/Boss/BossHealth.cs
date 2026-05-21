using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public float damagePerHit = 10f;
    public bool canBeHarmed = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;

    private void Start()
    {
        currentHealth = maxHealth;
        canBeHarmed = true;
    }

    public void TakeDamage()
    {
        if (!canBeHarmed || currentHealth <= 0) return;

        currentHealth -= damagePerHit;
        canBeHarmed = false; 

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        
        BossAI ai = GetComponent<BossAI>();

        if (currentHealth <= 0)
        {
            if (ai != null) 
            {
                ai.HandleDeath(); 
                ai.enabled = false;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.BossDied();
            }

            this.enabled = false; 
        }
        else
        {
            if (ai != null) ai.InterruptAndForceTeleport();
        }
    }

    public float GetHealthPercentage() => currentHealth / maxHealth;
}