using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour
{
    protected TextMeshProUGUI InteractionText;
    protected bool InRadius = false;

    [SerializeField] private GameObject _interactionBox;
    [SerializeField] private TextMeshProUGUI interactionText;

    void Start()
    {
        InteractionText = interactionText;
        
        interactionText.text = "Press " + InputManager.Instance.PlayerInputActions.Player.Interact.GetBindingDisplayString() + " to interact";
    }

    public virtual void Interact()
    {
        if (!InRadius) return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InRadius = true;
        _interactionBox.SetActive(true);
    }


    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        InRadius = false;
        _interactionBox.SetActive(false);
    }
}
