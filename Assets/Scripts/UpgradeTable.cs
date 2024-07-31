using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTable : Interactable
{
    public override void Interact()
    {
        Debug.Log("interact with upgrade table");
        //RespawnManager.Instance.SetRespawnTable(this);
        Player.Instance.Stats.Teleport = 2;

        Player.Instance.GetComponent<PlayerAbilityController>().UpdateTeleportNum(2);

        Player.Instance.GetComponent<PlayerHealth>()._playBuff = 0.8f;

    }
}
