using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AlchemyTable : Interactable
{
    public override void Interact()
    {
        Debug.Log("interact with table");
        Player.Instance.GetComponent<PlayerAbilityController>().SetDodgeAbility(true);
        Player.Instance.ResetPlayer();
    }
}
