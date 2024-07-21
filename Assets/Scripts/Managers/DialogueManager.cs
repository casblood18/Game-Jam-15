using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcDialogue;
    [SerializeField] private GameObject dialoguePanel;
    public NPCInteract NPC { get; set; }

    private bool dialogueStarted = false;
    private Queue<string> dialogueQueue = new Queue<string>();

    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        InputManager.InputInstance.OnInteractInput += ContinueDialogue;
    }
    private void OnDisable()
    {
        InputManager.InputInstance.OnInteractInput -= ContinueDialogue;
    }

    public void StartDialogue()
    {
        if (dialogueStarted) return;
        dialoguePanel.SetActive(true);
        dialogueStarted = true;
        LoadDialogue();
        npcName.text = NPC.npcDialogue.Name;
        npcDialogue.text = dialogueQueue.Peek();
    }
    private void LoadDialogue()
    {
        if (NPC.npcDialogue.Dialogue.Length <= 0) return;
        foreach (string sentence in NPC.npcDialogue.Dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }
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
        dialoguePanel.SetActive(false);
        dialogueQueue.Clear();
        dialogueStarted = false;
    }
}
