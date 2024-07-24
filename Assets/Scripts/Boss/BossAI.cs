using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public static Action<float> OnCheckStage;

    public Transform BossTransform => transform;
    public RedProjectile RedProjectile => redProjectile;
    public BlueProjectile BlueProjectile => blueProjectile;
    public YellowProjectile YellowProjectile => yellowProjectile;
    public PurpleProjectile PurpleProjectile => purpleProjectile;
    public OrangeProjectile OrangeProjectile => orangeProjectile;
    public GreenProjectile GreenProjectile => greenProjectile;

    [SerializeField] private StagesListSO stagesList;

    [Header("Projectile types")]
    [SerializeField] private RedProjectile redProjectile;
    [SerializeField] private BlueProjectile blueProjectile;
    [SerializeField] private YellowProjectile yellowProjectile;
    [SerializeField] private PurpleProjectile purpleProjectile;
    [SerializeField] private OrangeProjectile orangeProjectile;
    [SerializeField] private GreenProjectile greenProjectile;

    private StageBaseSO currentStage;
    private ushort currentStageIndex;

    private void OnEnable()
    {
        OnCheckStage += CheckStage;
    }

    private void OnDisable()
    {
        OnCheckStage -= CheckStage;
    }

    private void Start()
    {
        currentStageIndex = 0;
        currentStage = stagesList.Stages[currentStageIndex];

        StartCoroutine(currentStage.Attack(this));
    }

    private void CheckStage(float health)
    {
        StageBaseSO nextStage = stagesList.Stages[currentStageIndex + 1];

        if (health > nextStage.MinimumHealth) return;

        SwitchStage();
    }

    private void SwitchStage()
    {
        currentStageIndex++;
        currentStage = stagesList.Stages[currentStageIndex];

        StartCoroutine(currentStage.Attack(this));
    }
}
