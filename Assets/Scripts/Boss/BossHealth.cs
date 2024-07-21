using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : HealthBase
{
    public static Action<float> OnDamageBoss;

    [SerializeField] private BossStats stats;

    private void OnEnable()
    {
        OnDamageBoss += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamageBoss -= TakeDamage;
    }

    private void Awake()
    {
        stats.Health = stats.MaxHealth;
    }

    protected override void Death()
    {

    }

    protected override void TakeDamage(float amount)
    {
        stats.Health -= amount;

        if (stats.Health > 0f) return;
        
        Death();
    }
}
