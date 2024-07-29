
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageTaken
{
    private Player player;
    private float _playerHealth;
    private float _playerHealthMax;

    [SerializeField] HUD _HUD;

    private void Awake()
    {
        player = GetComponent<Player>();
        
        _playerHealth = player.Stats.MaxHealth;
        _playerHealthMax = player.Stats.MaxHealth;
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            DamageTaken(1f);
        }
    }
    public void DamageTaken(float damage)
    {
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

    private void PlayerDeath()
    {
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
