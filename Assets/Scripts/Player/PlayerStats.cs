using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player Stats")]
public class PlayerStats : StatsBaseSO
{
    [Header("Mana")]
    public float Mana;
    public float MaxMana;

    [ContextMenu("Reset player stats")]
    public void ResetStats()
    {
        Mana = MaxMana;
        Health = MaxHealth;
    }
}
