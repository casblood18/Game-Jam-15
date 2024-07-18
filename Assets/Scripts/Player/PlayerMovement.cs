using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed;
    private PlayerActions action;
    private Vector2 movementDirection;
    private Rigidbody2D rg2d;
    private PlayerAnimation playerAnimation;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>(); 
        action = new PlayerActions();
        rg2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
        movementDirection = action.Movement.Move.ReadValue<Vector2>().normalized;
        if (movementDirection == new Vector2(0, 0))
        {
            playerAnimation.SetMovingAnimation(false);
            return;
        }

        playerAnimation.SetMovingAnimation(true);
        playerAnimation.SetMoveDirection(movementDirection);
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void OnEnable()
    {
        action.Enable();
    }
}
