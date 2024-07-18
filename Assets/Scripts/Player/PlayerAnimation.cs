using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");
    private readonly int moving = Animator.StringToHash("IsMoving");
    private readonly int dead = Animator.StringToHash("Dead");
    private readonly int revive = Animator.StringToHash("Revive");

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
}
