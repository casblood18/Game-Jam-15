using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public NPCInteract NPC { get; set; }

    private bool dialogueStarted = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartDialogue()
    {
        if (dialogueStarted) return;
        dialogueStarted = true;
        Debug.Log("D STARTED");
    }

    public void EndDialogue()
    {
        dialogueStarted = false;
    }
}
