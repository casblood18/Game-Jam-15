using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
        public static InputManager InputInstance;

    [HideInInspector] public PlayerInputActions PlayerInputActions;

    public Action OnTeleportInput;
    public Action OnInteractInput;
    public Action OnHealInput;
    public Action OnRollInput;
    public Action OnAttackInput;

    private void Awake()
    {
        InputInstance = this;
        PlayerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.Move.Enable();
        PlayerInputActions.Player.Teleport.Enable();
        PlayerInputActions.Player.Heal.Enable();
        PlayerInputActions.Player.Interact.Enable();
        PlayerInputActions.Player.Roll.Enable();
        PlayerInputActions.Player.Attack.Enable();

        PlayerInputActions.Player.Teleport.performed += OnTeleport;
        PlayerInputActions.Player.Heal.performed += OnHeal;
        PlayerInputActions.Player.Interact.performed += OnInteract;
        PlayerInputActions.Player.Roll.performed += OnRoll;
        PlayerInputActions.Player.Attack.performed += OnAttack;
    }

    private void OnTeleport(InputAction.CallbackContext context)
    {
        OnTeleportInput?.Invoke();
    }
    private void OnHeal(InputAction.CallbackContext context)
    {
        OnHealInput?.Invoke();
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        OnInteractInput?.Invoke();
    }
    private void OnRoll(InputAction.CallbackContext context)
    {
        OnRollInput?.Invoke();
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.Move.Disable();
        PlayerInputActions.Player.Heal.Disable();
        PlayerInputActions.Player.Teleport.Disable();
        PlayerInputActions.Player.Interact.Disable();
        PlayerInputActions.Player.Roll.Disable();
        PlayerInputActions.Player.Attack.Disable();

        PlayerInputActions.Player.Teleport.performed -= OnTeleport;
        PlayerInputActions.Player.Heal.performed -= OnHeal;
        PlayerInputActions.Player.Interact.performed -= OnInteract;
        PlayerInputActions.Player.Roll.performed -= OnRoll;
        PlayerInputActions.Player.Attack.performed -= OnAttack;
    }
}
