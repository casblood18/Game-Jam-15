using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerAnimation animations;

    public PlayerStats Stats => stats;
    [HideInInspector] public SpriteRenderer playerSpriteRenderer;
    [HideInInspector] public PlayerAnimation playerAnimation;

    protected override void Awake()
    {
        base.Awake();
        playerSpriteRenderer = _spriteRenderer;
        playerAnimation = animations;
    }
    private void Start()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        stats.ResetStats();
        animations.SetRevive();
    }

}
