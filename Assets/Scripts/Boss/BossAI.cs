using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public static Action<float> OnCheckStage;

    [SerializeField] private StagesListSO stagesList;

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
    private void Awake()
    {
        currentStageIndex = 0;
        currentStage = stagesList.Stages[currentStageIndex];
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
    }
}
