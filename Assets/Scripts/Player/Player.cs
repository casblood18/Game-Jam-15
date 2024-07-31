using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Player : Singleton<Player>
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerAnimation animations;
    [SerializeField] private Transform footPosition;
    [SerializeField] private Animator _playerAnimator;
    public PlayerStats Stats => stats;
    public Transform FootPosition => footPosition;
    [HideInInspector] public SpriteRenderer playerSpriteRenderer => _spriteRenderer;
    [HideInInspector] public PlayerAnimation playerAnimation => animations;
    [HideInInspector] public Animator playerAnimator => _playerAnimator;
    [HideInInspector] public PlayerMovement playerMovement => this.GetComponent<PlayerMovement>();
    [HideInInspector] public PlayerAbilityController playerAbilityController => this.GetComponent<PlayerAbilityController>();
    [HideInInspector] public PlayerHealth playerHealth => this.GetComponent<PlayerHealth>();
    protected override void Awake()
    {
        base.Awake();
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
