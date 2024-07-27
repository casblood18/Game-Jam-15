using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private HUD _HUD;

    public NPCInteract NPC { get; set; }

    private Queue<string> dialogueQueue = new Queue<string>();
    private bool dialogueStarted = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnInteractInput += ContinueDialogue;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteractInput -= ContinueDialogue;
    }

    public void LoadDialogue()
    {
        if (NPC.npcDialogue.Dialogue.Length <= 0) return;
        foreach (string sentence in NPC.npcDialogue.Dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }
    }

    public void StartDialogue()
    {
        if (dialogueStarted) return;
        LoadDialogue();
        _HUD.SetDialogueUI(true);
        dialogueStarted = true;
        _HUD.SetDialogueNPC(NPC.npcDialogue.Name, NPC.npcDialogue.Avatar);
        _HUD.SetDialogueContext(dialogueQueue.Peek());
    }

    private void ContinueDialogue()
    {
        if (NPC == null)
        {
            dialogueQueue.Clear();
            return;
        }

        if (dialogueQueue.Count <= 0)
        {
            EndDialogue();
            dialogueStarted = false;
            return;
        }

        _HUD.SetDialogueContext(dialogueQueue.Dequeue());
    }

    public void EndDialogue()
    {
        dialogueStarted = false;
        _HUD.SetDialogueUI(false);
        dialogueQueue.Clear();
    }
}
