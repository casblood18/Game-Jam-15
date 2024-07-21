using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "BossStage/Stage", order = 1)]
public abstract class StageBaseSO : ScriptableObject
{
    public float MinimumHealth;

    public abstract void Attack();
    public abstract void Movement();
}
