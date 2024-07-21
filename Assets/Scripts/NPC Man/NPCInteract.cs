using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactionBox;
    private bool inRadius = false;


    private void OnEnable()
    {
        InputManager.instance.OnInteractInput += InteractNPC;
    }

    private void InteractNPC()
    {
        if (inRadius)
        {
            DialogueManager.Instance.StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.NPC = this;
            inRadius = true;
            interactionBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRadius = false;
            DialogueManager.Instance.NPC = null;
            DialogueManager.Instance.EndDialogue();
            interactionBox.SetActive(false);
        }
    }
}
