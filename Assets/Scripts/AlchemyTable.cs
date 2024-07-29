using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlchemyTable : Interactable
{
    public override void Interact()
    {
        Debug.Log("interact with table");
        Player.Instance.ResetPlayer();
    }
}
