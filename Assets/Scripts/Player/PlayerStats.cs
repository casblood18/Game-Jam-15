using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Mana")]
    public float Mana;
    public float MaxMana;

    public void ResetStats()
    {
        Mana = MaxMana;
        Health = MaxHealth;
    }
}
