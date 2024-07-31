using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunPath : MonoBehaviour
{
    //please, for the gamejam sakes im creating singletons, but in real life, your game shouldnt be depended on singletons
    public static WallRunPath Instance;

    [SerializeField] private List<GameObject> wallRunPoints;
    [SerializeField] private List<GameObject> circlePathPoints;


    private short currentPointIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    public Transform MoveToNextPoint()
    {
        currentPointIndex = (short)((currentPointIndex + 1) % wallRunPoints.Count);

        return wallRunPoints[currentPointIndex].transform;
    }

    public Transform MoveToNextCirclePoint()
    {
        currentPointIndex = (short)((currentPointIndex + 1) % circlePathPoints.Count);

        return circlePathPoints[currentPointIndex].transform;
    }

    public List<GameObject> GetPoints() => wallRunPoints;

    public List<GameObject> GetCirclePoints() => circlePathPoints;
}
