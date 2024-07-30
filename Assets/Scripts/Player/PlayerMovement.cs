using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed;

    bool _isPlaying;
    public Vector2 movementDirection => InputManager.Instance.PlayerInputActions.Player.Move.ReadValue<Vector2>().normalized;

    private Rigidbody2D rg2d;
    private Player player;
    public bool CanMove = true;

    private void Awake()
    {
        player = GetComponent<Player>();
        rg2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (CanMove)
            ReadMovement();
    }

    private void FixedUpdate()
    {
        if (CanMove)
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
            player.playerAnimation.SetMovingAnimation(false);
            _isPlaying = false;
            SoundManager.Instance.StopSound(Audio.footstep);


            return;
        }

        player.playerAnimation.SetMovingAnimation(true);
        player.playerAnimation.SetMoveDirection(movementDirection);

        if (!_isPlaying) SoundManager.Instance.PlaySoundLooped(Audio.footstep);
        _isPlaying = true;
        
    }

}
