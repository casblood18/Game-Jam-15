using UnityEngine;

public class AlchemyTable : Interactable
{
    public override void Interact()
    {
        Debug.Log("interact with table");
        RespawnManager.Instance.SetRespawnTable(this);
        Player.Instance.GetComponent<PlayerAbilityController>().SetDodgeAbility(true);
        Player.Instance.ResetPlayer();
    }
}
