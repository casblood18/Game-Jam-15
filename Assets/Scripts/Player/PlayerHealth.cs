using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageTaken
{
    private Player player;
    private PlayerAnimation playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();
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
        player.Stats.Health -= damage;
        if (player.Stats.Health <= 0f)
        {
            PlayerDeath();
        }

    }

    private void PlayerDeath()
    {
        playerAnimator.SetDeadAnimation();
    }
}
