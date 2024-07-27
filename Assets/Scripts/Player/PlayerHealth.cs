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

    [SerializeField] HUD _HUD;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();

        _playerHealth = player.Stats.MaxHealth;
    }

    private void OnEnable()
    {
        OnDamagePlayer += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamagePlayer -= TakeDamage;
    }

    protected override void TakeDamage(float damage)
    {
        _HUD.OnPlayerHealthChanged(_playerHealth);

       _playerHealth -= damage;

        if (_playerHealth> 0f) return;

        Death();
    }

    protected override void Death()
    {
        playerAnimator.SetDeadAnimation();
    }

}
