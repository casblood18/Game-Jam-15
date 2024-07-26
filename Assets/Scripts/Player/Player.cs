using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : Singleton<Player>, IDamageTaken
{
    [SerializeField] private PlayerStats stats;
    private PlayerAnimation animations;

    public PlayerStats Stats => stats;
    public SpriteRenderer playerSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        animations = GetComponent<PlayerAnimation>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetPlayer()
    {
        animations.SetRevive();
        stats.ResetStats();
    }

    public void DamageTaken(float amount)
    {
        stats.Health -= amount;
    }
}
