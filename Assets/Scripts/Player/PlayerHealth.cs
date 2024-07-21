using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthBase
{
    public static Action<float> OnDamagePlayer;
    private Player player;
    private PlayerAnimation playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();

        player.Stats.Health = player.Stats.MaxHealth;
    }

    private void OnEnable()
    {
        OnDamagePlayer += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamagePlayer -= TakeDamage;
    }

    protected override void TakeDamage(float amount)
    {
        player.Stats.Health -= amount;

        if (player.Stats.Health > 0f) return;

        Death();
    }

    protected override void Death()
    {
        playerAnimator.SetDeadAnimation();
    }
}
