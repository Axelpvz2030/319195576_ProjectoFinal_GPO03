using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; 

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    
    public int maxHealCharges = 3;
    public int currentHealCharges;
    public float healAmount = 30f;
    public float healCooldown = 2f;
    private float lastHealTime = -Mathf.Infinity;

    public float invulnerabilityTime = 1.5f;
    private bool isInvulnerable = false;
    
    public GameObject playerModel; 
    public float flashInterval = 0.1f;

    public BattleHUD _hudmanager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip healSound;

    [Header("VFX")]
    public ParticleSystem healParticles;

    void Start()
    {
        currentHealth = maxHealth;
        currentHealCharges = maxHealCharges;
        
        if (_hudmanager != null)
        {
            _hudmanager.UpdatePlayerHUD();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AttemptHeal();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); 
        
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (_hudmanager != null)
        {
            _hudmanager.UpdatePlayerHUD();
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        
        float timer = 0f;
        bool isVisible = true;

        while (timer < invulnerabilityTime)
        {
            if (playerModel != null)
            {
                isVisible = !isVisible;
                playerModel.SetActive(isVisible);
            }

            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        if (playerModel != null)
        {
            playerModel.SetActive(true);
        }

        isInvulnerable = false;
    }

    private void AttemptHeal()
    {
        if (currentHealCharges > 0 && Time.time >= lastHealTime + healCooldown && currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth); 
            
            currentHealCharges--;
            lastHealTime = Time.time;

            if (audioSource != null && healSound != null)
            {
                audioSource.PlayOneShot(healSound);
            }

            if (healParticles != null)
            {
                healParticles.Play();
            }
            
            if (_hudmanager != null)
            {
                _hudmanager.UpdatePlayerHUD();
            }
        }
    }

    private void HandleDeath()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public float GetHealCooldownPercentage()
    {
        if (Time.time >= lastHealTime + healCooldown)
        {
            return 0f; 
        }
        return 1f - ((Time.time - lastHealTime) / healCooldown);
    }

    public int GetHealsRemaining()
    {
        return currentHealCharges;
    }
}