using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStats Stats => stats;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerAnimation animations;
    [SerializeField] private Transform footPosition;
    public PlayerStats Stats => stats;
    public Transform FootPosition => footPosition;
    [HideInInspector] public SpriteRenderer playerSpriteRenderer;
    [HideInInspector] public PlayerAnimation playerAnimation;
    public SpriteRenderer playerSpriteRenderer;


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
