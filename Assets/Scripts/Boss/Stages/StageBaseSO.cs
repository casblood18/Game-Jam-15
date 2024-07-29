using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBaseSO : ScriptableObject
{
    public float MinimumHealth;

    private List<ProjectileBaseSO> projectiles;
    private int maxProjectilesPerType;
    private Dictionary<ProjectileBaseSO, int> usageCounts;


    public abstract IEnumerator Attack(BossAI boss);
    public abstract void Movement(BossAI boss);

 

    protected ProjectileBaseSO GetRandomProjectile(BossAI boss, int maxProjectiles, bool isNewStage)
    {
        if (projectiles.Count == 0)
        {
            Initialize(maxProjectiles, boss);
        }

        if (isNewStage)
        {
            usageCounts = new Dictionary<ProjectileBaseSO, int>
            {
                { boss.BlueProjectile, 0 },
                { boss.RedProjectile, 0 },
                { boss.YellowProjectile, 0 }
            };
        }

        List<ProjectileBaseSO> eligibleProjectiles = new();

        foreach (ProjectileBaseSO projectile in projectiles)
        {
            if (usageCounts[projectile] < maxProjectilesPerType)
            {
                eligibleProjectiles.Add(projectile);
            }
        }

        if (eligibleProjectiles.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, eligibleProjectiles.Count);
        ProjectileBaseSO selectedProjectile = eligibleProjectiles[randomIndex];

        usageCounts[selectedProjectile]++;

        return selectedProjectile;
    }

    private void Initialize(int maxProjectiles, BossAI boss)
    {
        maxProjectilesPerType = maxProjectiles / 3;

        projectiles = new List<ProjectileBaseSO> {
            boss.BlueProjectile,
            boss.RedProjectile,
            boss.YellowProjectile
        };

        usageCounts = new Dictionary<ProjectileBaseSO, int>
        {
            { boss.BlueProjectile, 0 },
            { boss.RedProjectile, 0 },
            { boss.YellowProjectile, 0 }
        };
    }

       [ContextMenu("Reset projectiles")]
    private void ResetProjectiles()
    {
        projectiles = new();
    }
}
