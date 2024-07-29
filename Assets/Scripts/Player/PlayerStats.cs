using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Teleport")]
    public int Teleport;

    [Header("Mana")]
    public float Mana;
    public float MaxMana;


    public Action OnResetPlayerStats;
    public void ResetStats()
    {
        Debug.Log("reset");
        Mana = MaxMana;
        Health = MaxHealth;
        Teleport = 1;
        OnResetPlayerStats?.Invoke();
    }
}
