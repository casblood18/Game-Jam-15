using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player Stats")]
public class PlayerStats : StatsBaseSO
{
    [Header("Teleport")]
    public int Teleport;

    [Header("Mana")]
    public float Mana;
    public float MaxMana;


    public Action OnResetPlayerStats;
    [ContextMenu("Reset player stats")]
    public void ResetStats()
    {
        Debug.Log("reset");
        Mana = MaxMana;
        Health = MaxHealth;
        Teleport = 1;
        OnResetPlayerStats?.Invoke();
    }
}
