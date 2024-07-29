using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private HUD _HUD;

    public NPCInteract NPC { get; set; }

    private Queue<string> dialogueQueue = new Queue<string>();
    private bool dialogueStarted = false;

    public Action OnDialogueEnd;
    protected override void Awake()
    {
        base.Awake();
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
        SoundManager.Instance.StopSound(Audio.dialogue);

        if (dialogueStarted) 
        {
            //Debug.Log("ContinueDialogue");
            ContinueDialogue(); 
        }
        else
        {
            //Debug.Log("start new dialogue");
            LoadDialogue();
            _HUD.SetDialogueUI(true);
            dialogueStarted = true;
            _HUD.SetDialogueNPC(NPC.npcDialogue.Name, NPC.npcDialogue.Avatar);
            SoundManager.Instance.PlaySoundOnce(Audio.interact);
            _HUD.SetDialogueContext(dialogueQueue.Dequeue());
            SoundManager.Instance.PlaySoundOnce(Audio.dialogue);
        }
        
    }

    private void ContinueDialogue()
    {
        if (NPC == null)
        {
            ResetDialogue();
            return;
        }
        if (dialogueQueue.Count <= 0)
        {
            NPC.DialogueEnd();
            ResetDialogue();
            return;
        }
        SoundManager.Instance.PlaySoundOnce(Audio.interact);
        _HUD.SetDialogueContext(dialogueQueue.Dequeue());
        SoundManager.Instance.PlaySoundOnce(Audio.dialogue);
    }

    public void ResetDialogue()
    {
        if (!dialogueStarted) return;
        dialogueStarted = false;
        _HUD.SetDialogueUI(false);
        dialogueQueue.Clear();
        Debug.Log("deactivate dialogue");
    }
}
