using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed;

    private Vector2 movementDirection => InputManager.Instance.PlayerInputActions.Player.Move.ReadValue<Vector2>().normalized;

    private Rigidbody2D rg2d;
    private PlayerAnimation playerAnimation;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
        
        rg2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ReadMovement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (player.Stats.Health <= 0) return;
        rg2d.MovePosition(rg2d.position + movementDirection * (moveSpeed * Time.fixedDeltaTime));
    }

    private void ReadMovement()
    {
        if (movementDirection == new Vector2(0, 0))
        {
            playerAnimation.SetMovingAnimation(false);
            return;
        }

        playerAnimation.SetMovingAnimation(true);
        playerAnimation.SetMoveDirection(movementDirection);
    }

}
