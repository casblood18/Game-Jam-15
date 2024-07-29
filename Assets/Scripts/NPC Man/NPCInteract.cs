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
        if (inRadius)
            DialogueManager.Instance.StartDialogue();
    }
    public virtual void DialogueEnd()
    {
        Debug.Log($"end dialogue with {npcDialogue.name}");
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
        
    }

    public override void Interact()
    {
        base.Interact();
        StartDialogue();
    }
}
