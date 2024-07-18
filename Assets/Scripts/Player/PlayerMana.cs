using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    private Player player;

    public void Awake()
    {
        player = GetComponent<Player>();
    }

    public void UseMana(float amount)
    {
        player.Stats.Mana = Mathf.Max(player.Stats.Mana -= amount, 0f);
    }
}
