using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_slashAttack : BossAttack
{
    public GameObject slashPrefab;
    public Animator bossAnimator;
    public int numberOfSpawns = 3;
    public float timeBetweenSpawns = 1.5f;
    public int poolSize = 20;

    private List<GameObject> slashPool;
    private bool isAttacking;

    private void Start()
    {
        slashPool = new List<GameObject>();
        
        if (slashPrefab != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(slashPrefab);
                obj.SetActive(false);
                slashPool.Add(obj);
            }
        }
    }

    public override IEnumerator ExecuteAttack(BossAI bossAI)
    {
        isAttacking = true;

        if (bossAnimator == null && bossAI != null)
        {
            bossAnimator = bossAI.GetComponent<Animator>();
        }

        for (int i = 1; i <= numberOfSpawns; i++)
        {
            if (!isAttacking) break;

            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("slash");
            }

            float baseAngle = Random.Range(0f, 360f);
            float angleStep = 360f / (i * 2f);

            for (int j = 0; j < i; j++)
            {
                GameObject slash = GetSlashFromPool();
                
                if (slash != null && bossAI.player != null)
                {
                    slash.transform.position = bossAI.player.position;
                    slash.transform.rotation = Quaternion.Euler(0f, baseAngle + (j * angleStep), 0f);
                    slash.SetActive(true);
                }
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isAttacking = false;
    }

    private GameObject GetSlashFromPool()
    {
        foreach (GameObject slash in slashPool)
        {
            if (!slash.activeInHierarchy)
            {
                return slash;
            }
        }
        return null;
    }

    public override void CancelAttack()
    {
        isAttacking = false;
        
        foreach (GameObject slash in slashPool)
        {
            if (slash != null)
            {
                slash.SetActive(false);
            }
        }
    }
}