using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstNPC : NPCInteract
{
    [SerializeField] GameObject _bridgeBlock;
    public override void DialogueEnd()
    {
        base.DialogueEnd();
        _bridgeBlock.SetActive(false);
        Player.Instance.GetComponent<PlayerAbilityController>().SetDodgeAbility(true);
    }
}
