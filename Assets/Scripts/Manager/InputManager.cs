using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    [HideInInspector] public PlayerInputActions PlayerInputActions;

    public Action OnTeleportInput;
    public Action OnInteractInput;
    public Action OnHealInput;
    public Action OnDodgeInput;
    public Action OnAttackInput;

    private void OnEnable()
    {
        LoadInput();
        PlayerInputActions.Enable();

        PlayerInputActions.Player.Attack.performed += OnAttack;
        PlayerInputActions.Player.Teleport.performed += OnTeleport;
        PlayerInputActions.Player.Heal.performed += OnHeal;
        PlayerInputActions.Player.Interact.performed += OnInteract;
        PlayerInputActions.Player.Roll.performed += OnRoll;
    }

    private void OnDisable()
    {
        PlayerInputActions.Disable();

        PlayerInputActions.Player.Teleport.performed -= OnTeleport;
        PlayerInputActions.Player.Heal.performed -= OnHeal;
        PlayerInputActions.Player.Interact.performed -= OnInteract;
        PlayerInputActions.Player.Roll.performed -= OnRoll;
        PlayerInputActions.Player.Attack.performed -= OnAttack;
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
        OnDodgeInput?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        OnAttackInput?.Invoke();
    }

    private void LoadInput()
    {
        PlayerInputActions = new PlayerInputActions();

        string rebinds = PlayerPrefs.GetString("rebinds");

        if (!string.IsNullOrEmpty(rebinds)) PlayerInputActions.asset.LoadBindingOverridesFromJson(rebinds);
    }
}
