using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        animator.SetTrigger(revive);
    }

    public void TeleportInAnimation()
    {
        animator.Play("Teleport In");
    }

    public void Teleport()
    {
        playerAbilityController.TeleportOut();
        TeleportOutAnimation();
    }

    public void TeleportOutAnimation()
    {
        animator.Play("Teleport Out");
    }

    public void SetAttackAnimation()
    {
        animator.SetTrigger(attack);
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

    public void RotateNormal()
    {
        playerAbilityController.FlipXNormal();
    }
}