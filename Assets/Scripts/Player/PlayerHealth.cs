using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerHealth : HealthBase
{
    public static Action<float> OnDamagePlayer;
    private Player player;
    private PlayerAnimation playerAnimator;
    private float _playerHealth;
    private float _playerHealthMax;

    [SerializeField] HUD _HUD;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();
        player.Stats.Health = player.Stats.MaxHealth;
        _playerHealth = player.Stats.MaxHealth;
        _playerHealthMax = player.Stats.MaxHealth;
    }

    private void OnEnable()
    {
        OnDamagePlayer += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamagePlayer -= TakeDamage;

        _playerHealth -= damage;
        _HUD.OnPlayerHealthChanged(_playerHealth);

        UpdatePlayerStat();
        if (_playerHealth <= 0f)
        {
            PlayerDeath();
        }
    }
    
    private void UpdatePlayerStat()
    {
        player.Stats.Health = _playerHealth;
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
