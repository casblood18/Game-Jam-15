using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : HealthBase
{
    public static Action<float> OnDamagePlayer;
    private Player player;
    private float _playerHealth;

    [SerializeField] HUD _HUD;

    private void Awake()
    {
        player = GetComponent<Player>();
        
        _playerHealth = player.Stats.MaxHealth;
    }
    private void OnEnable()
    {
        player.Stats.OnResetPlayerStats += ResetPlayerHealth;
    }
    private void OnDisable()
    {
        player.Stats.OnResetPlayerStats -= ResetPlayerHealth;
    }

    private void Update()
    {
        OnDamagePlayer += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamagePlayer -= TakeDamage;
    }

    protected override void TakeDamage(float damage)
    {
        if (_HUD != null)
            _HUD.OnPlayerHealthChanged(_playerHealth);

        _playerHealth -= damage;

        if (_playerHealth > 0f) return;

        Death();
    }

    protected override void Death()
    {
        Player.Instance.GetComponent<PlayerAbilityController>().SetDodgeAbility(false);
        Player.Instance.playerAnimation.SetDeadAnimation();
        //go to alchemy table
        Debug.Log(RespawnManager.Instance);
        RespawnManager.Instance.RespawnPlayer();


    }

    private void ResetPlayerHealth()
    {
        Debug.Log("reset player health to " + player.Stats.Health);
        _playerHealth = player.Stats.Health;
        _HUD.OnPlayerHealthChanged(_playerHealth);
    }

}
