using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStats Stats => stats;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerAnimation animations;
    [HideInInspector] public PlayerAnimation playerAnimation;
    public SpriteRenderer playerSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        playerSpriteRenderer = _spriteRenderer;
        playerAnimation = animations;
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        stats.ResetStats();
        animations.SetRevive();
    }
}
