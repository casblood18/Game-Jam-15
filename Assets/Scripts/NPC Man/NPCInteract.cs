using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteract : Interactable
{
    [SerializeField] private NPCDialogue _npcDialogue;

    public NPCDialogue npcDialogue => _npcDialogue;


    private void StartDialogue()
    {
        if (!InRadius) return;

        DialogueManager.Instance.StartDialogue();
    }

    public virtual void DialogueEnd()
    {
        DialogueManager.IsDialogueActivated = false;

        Debug.Log($"end dialogue with {npcDialogue.name}");

        InteractionText.text = "Press " + InputManager.Instance.PlayerInputActions.Player.Interact.GetBindingDisplayString() + " to interact";
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        DialogueManager.Instance.NPC = this;
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        DialogueManager.Instance.NPC = null;
        DialogueManager.Instance.ResetDialogue();

        DialogueManager.IsDialogueActivated = false;
        InteractionText.text = "Press " + InputManager.Instance.PlayerInputActions.Player.Interact.GetBindingDisplayString() + " to interact";

    }

    public override void Interact()
    {
        base.Interact();

        if (PauseMenu.IsPauseMenuEnabled) return;

        InteractionText.text = "Press " + InputManager.Instance.PlayerInputActions.Player.Interact.GetBindingDisplayString() + " to continue dialogue";

        StartDialogue();
    }
}
