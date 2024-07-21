using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagesList", menuName = "Stages/StagesList", order = 1)]
public class StagesListSO : ScriptableObject
{
    public List<StageBaseSO> Stages;
}
