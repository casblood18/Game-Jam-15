using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    [SerializeField] private PlayerStats stats;
    private PlayerAnimation animations;

    public PlayerStats Stats => stats;

    private void Awake()
    {
        animations = GetComponent<PlayerAnimation>();
    }

    public void ResetPlayer()
    {
        animations.SetRevive();
        stats.ResetStats();
    }

    public void DamageTaken(float amount)
    {
        stats.Health -= amount;
    }
}
