using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StageBaseSO : ScriptableObject
{
    public float MinimumHealth;

    public abstract IEnumerator Attack(BossAI boss);
    public abstract void Movement(BossAI boss);

    protected ProjectileBaseSO GetRandomProjectile(BossAI boss)
    {
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                return boss.BlueProjectile;
            case 1:
                return boss.RedProjectile;
            case 2:
                return boss.YellowProjectile;
            default:
                return null;
        }
    }
}
