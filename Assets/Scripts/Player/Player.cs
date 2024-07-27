using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStats Stats => stats;

    [SerializeField] private PlayerStats stats;
    private PlayerAnimation animations;

    public SpriteRenderer playerSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        animations = GetComponent<PlayerAnimation>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        stats.ResetStats();
        animations.SetRevive();
    }
}
