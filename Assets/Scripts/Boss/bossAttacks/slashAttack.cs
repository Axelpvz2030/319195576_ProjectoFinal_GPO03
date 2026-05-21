using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class SlashAttack : MonoBehaviour
{
    public float spinSpeed = 720f;
    public float spinDuration = 1f;
    public float pauseBeforeActive = 0.2f;
    public float activeDuration = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip slashSound;

    private Hurtbox hurtbox;

    private void Awake()
    {
        hurtbox = GetComponent<Hurtbox>();
    }

    private void OnEnable()
    {
        if (hurtbox != null)
        {
            hurtbox.isActive = false;
        }
        
        StartCoroutine(ExecuteSlash());
    }

    private IEnumerator ExecuteSlash()
    {
        float timer = 0f;

        while (timer < spinDuration)
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(pauseBeforeActive);

        if (hurtbox != null)
        {
            hurtbox.isActive = true;

            if (audioSource != null && slashSound != null)
            {
                audioSource.PlayOneShot(slashSound);
            }
        }

        yield return new WaitForSeconds(activeDuration);

        gameObject.SetActive(false);
    }
}