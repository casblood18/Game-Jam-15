using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    
    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");
    private readonly int moving = Animator.StringToHash("IsMoving");
    private readonly int dead = Animator.StringToHash("Dead");
    private readonly int revive = Animator.StringToHash("Revive");
    private readonly int attack = Animator.StringToHash("Attack");
    private readonly int teleport = Animator.StringToHash("Teleporting");
    private readonly int roll = Animator.StringToHash("Roll");

    private Animator animator;
    private PlayerAbilityController playerAbilityController;
 
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerAbilityController = Player.Instance.GetComponent<PlayerAbilityController>();
    }
    public void SetDeadAnimation()
    {
        SoundManager.Instance.PlaySoundOnce(Audio.dead);
        animator.SetTrigger(dead);
    }

    public void SetMovingAnimation(bool value)
    {
        animator.SetBool(moving, value);
    }

    public void SetMoveDirection(Vector2 movementDirection)
    {
        animator.SetFloat(moveX, movementDirection.x);
        animator.SetFloat(moveY, movementDirection.y);
    }

    public void SetRevive()
    {
        SetMoveDirection(Vector2.down);
        if (Player.Instance.GetComponent<PlayerHealth>().PlayerIsDead)
        {
            Player.Instance.playerAbilityController.MovingAbilityPreparation();
        }
        animator.SetTrigger(revive);
    }

    public void TeleportInAnimation()
    {
        SoundManager.Instance.PlaySoundOnce(Audio.teleportIn);
        animator.Play("Teleport In");
    }

    public void TeleportOut()
    {
        Debug.Log("teleport out");
        SoundManager.Instance.PlaySoundOnce(Audio.teleportOut);
        animator.Play("TeleportOut");
        playerAbilityController.TeleportOut();
    }

    public void SetAttackAnimation()
    {
        SoundManager.Instance.PlaySoundOnce(Audio.attack);
        //animator.SetTrigger(attack);
    }

    public void PlayIdle()
    {
        animator.SetBool(attack, false);
        //animator.Play("Idle Animation");
    }

    public void RollAnimation()
    {
        animator.SetTrigger(roll);
    }

    public void DodgeAnimationFinish()
    {
        Player.Instance.playerAbilityController.EnableDodgeAfterAnimation();
    }
    public void DeadAnimationFinished()
    {
        SoundManager.Instance.StopSound(Audio.dead);
        Debug.Log("finishAnimationFrom Action");
        Player.Instance.playerAbilityController.MovingAbilityFinish();
    }
    public void MovingAbilityFinish()
    {
        Debug.Log("finishAnimationFrom Action");
        Player.Instance.playerAbilityController.MovingAbilityFinish();
    }

    public void FlipXNormal()
    {
        Player.Instance.playerSpriteRenderer.flipX = false;
    }
}