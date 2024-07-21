using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcDialogue;
    public NPCInteract NPC { get; set; }

    private Queue<string> dialogueQueue = new Queue<string>();
    private bool dialogueStarted = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        InputManager.instance.OnInteractInput += ContinueDialogue;
    }

    private void OnDisable()
    {
        InputManager.instance.OnInteractInput -= ContinueDialogue;
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
        dialoguePanel.SetActive(true);
        dialogueStarted = true;
        npcName.text = NPC.npcDialogue.Name;
        npcDialogue.text = dialogueQueue.Peek();
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

        npcDialogue.text = dialogueQueue.Dequeue();
    }

    public void EndDialogue()
    {
        dialogueStarted = false;
        dialoguePanel.SetActive(false);
        dialogueQueue.Clear();
    }
}
