using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Dialogue", fileName = "NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Avatar")]
    public string Name;
    public Sprite Icon;

    [Header("Dialogue")]
    [TextArea] public string[] Dialogue;
}
