using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactImage;
    [SerializeField] private NPCDialogue _npcDialogue;

    private bool inRadius = false;

    public NPCDialogue npcDialogue => _npcDialogue;

    private void OnEnable()
    {
        InputManager.InputInstance.OnInteractInput += InteractNPC;
    }

    //private void Start()
    //{
    //    interactionText.text = $"Press {InputManager.InputInstance.PlayerInputActions.Player.Interact.GetBindingDisplayString()} to Talk";
    //}

    private void InteractNPC()
    {
        if (!inRadius) return;

        DialogueManager.Instance.StartDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        DialogueManager.Instance.NPC = this;
        inRadius = true;
        interactImage.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadius = false;
            DialogueManager.Instance.NPC = null;
            DialogueManager.Instance.EndDialogue();
            interactImage.SetActive(false);
        }
    }
}
