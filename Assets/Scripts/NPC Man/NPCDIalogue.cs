using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCDialogue", fileName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public Sprite Avatar;

    [Header("Dialogue")]
    [TextArea] public string[] Dialogue;
}
