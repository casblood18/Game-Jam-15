using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions playerInputActions;
    public static InputManager instance;

    public Action OnTeleportInput;
    public Action OnInteractInput;
    public Action OnHealInput;
    public Action OnRollInput;
    public Action OnAttackInput;

    private void Awake()
    {
        instance = this;
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Teleport.Enable();
        playerInputActions.Player.Heal.Enable();
        playerInputActions.Player.Interact.Enable();
        playerInputActions.Player.Roll.Enable();
        playerInputActions.Player.Attack.Enable();

        playerInputActions.Player.Teleport.performed += OnTeleport;
        playerInputActions.Player.Heal.performed += OnHeal;
        playerInputActions.Player.Interact.performed += OnInteract;
        playerInputActions.Player.Roll.performed += OnRoll;
        playerInputActions.Player.Attack.performed += OnAttack;
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
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Heal.Disable();
        playerInputActions.Player.Teleport.Disable();
        playerInputActions.Player.Interact.Disable();
        playerInputActions.Player.Roll.Disable();
        playerInputActions.Player.Attack.Disable();

        playerInputActions.Player.Teleport.performed -= OnTeleport;
        playerInputActions.Player.Heal.performed -= OnHeal;
        playerInputActions.Player.Interact.performed -= OnInteract;
        playerInputActions.Player.Roll.performed -= OnRoll;
        playerInputActions.Player.Attack.performed -= OnAttack;
    }
}
