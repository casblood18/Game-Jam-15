using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] private NPCDialogue _npcDialogue;
    [SerializeField] private GameObject interactionBox;
    [SerializeField] private TextMeshProUGUI interactionText;

    private bool inRadius = false;
    public NPCDialogue npcDialogue => _npcDialogue;

    public virtual void DialogueEnd()
    {
        Debug.Log($"end dialogue with {npcDialogue.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        DialogueManager.Instance.NPC = this;
        inRadius = true;
        interactionBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadius = false;
            DialogueManager.Instance.NPC = null;
            interactionBox.SetActive(false);
            DialogueManager.Instance.ResetDialogue();
        }
    }
}
