using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStats Stats => stats;

    [SerializeField] private PlayerStats stats;
    private PlayerAnimation animations;

    private void Awake()
    {
        animations = GetComponent<PlayerAnimation>();
    }

    public void ResetPlayer()
    {
        animations.SetRevive();
        stats.ResetStats();
    }
}
