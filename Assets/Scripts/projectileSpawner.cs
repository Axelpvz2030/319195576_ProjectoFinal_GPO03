using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner : MonoBehaviour
{
    public bool isActive = true; 
    public float spawnTime = 1f;
    public GameObject projectileModel;
    public GameObject projectilePrefab;

    public int poolSize = 10;
    private List<GameObject> projectilePool;

    private float currentTimer = 0f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    private void Start()
    {
        if (projectileModel != null) projectileModel.SetActive(isActive);

        projectilePool = new List<GameObject>();
        
        if (projectilePrefab != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(projectilePrefab);
                obj.SetActive(false);
                projectilePool.Add(obj);
            }
        }
    }

    public void SetSpawnerActive(bool state)
    {
        if (state && spawnTime <= 0f)
        {
            SpawnProjectile();
            return;
        }

        isActive = state;
        
        if (!isActive) 
        {
            currentTimer = 0f;
            if (projectileModel != null) projectileModel.SetActive(false);
        }
        else 
        {
            if (projectileModel != null) projectileModel.SetActive(true);
        }
    }

    private void Update()
    {
        if (!isActive) return;

        currentTimer += Time.deltaTime;
        if (currentTimer >= spawnTime)
        {
            SpawnProjectile();
            SetSpawnerActive(false); 
        }
    }

    private void SpawnProjectile()
    {
        foreach (GameObject projectile in projectilePool)
        {
            if (!projectile.activeInHierarchy)
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = transform.rotation;
                projectile.SetActive(true);
                
                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }

                Hurtbox[] hurtboxes = projectile.GetComponentsInChildren<Hurtbox>(true);
                foreach (Hurtbox hurtbox in hurtboxes)
                {
                    hurtbox.gameObject.SetActive(true);
                }
                
                ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();
                if (movement != null)
                {
                    movement.Shoot();
                }
                
                return; 
            }
        }
    }
}