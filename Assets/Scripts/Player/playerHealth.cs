using UnityEngine;
using UnityEngine.InputSystem; // Added this to use the New Input System!

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    
    public int maxHealCharges = 3;
    public int currentHealCharges;
    public float healAmount = 30f;
    public float healCooldown = 2f;
    private float lastHealTime = -Mathf.Infinity;

    public BattleHUD _hudmanager;

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
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); 
        
        if (_hudmanager != null)
        {
            _hudmanager.UpdatePlayerHUD();
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void AttemptHeal()
    {
        if (currentHealCharges > 0 && Time.time >= lastHealTime + healCooldown && currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth); 
            
            currentHealCharges--;
            lastHealTime = Time.time;
            
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
        else
        {
            Debug.LogError("GameManager is missing from the scene!");
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