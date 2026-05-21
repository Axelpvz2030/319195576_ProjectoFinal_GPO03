using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; 

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject attackHurtbox; 
    public float attackDuration = 0.5f;
    
    [Header("Components")]
    public Animator animator; 

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip slashSound;

    public bool isAttacking = false; 
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        
        if (attackHurtbox != null)
        {
            attackHurtbox.SetActive(false);
        }
    }

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }

        bool isShielding = (playerMovement != null && playerMovement.isShielding);

        if (!isAttacking && !isShielding && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        
        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        if (playerMovement != null) playerMovement.canMove = false;
        if (attackHurtbox != null) attackHurtbox.SetActive(true);

        if (animator != null) animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);

        if (attackHurtbox != null) attackHurtbox.SetActive(false);
        if (playerMovement != null) playerMovement.canMove = true;

        isAttacking = false;
    }
}