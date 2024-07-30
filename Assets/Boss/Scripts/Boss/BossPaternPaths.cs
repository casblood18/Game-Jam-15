using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPaternPaths : MonoBehaviour
{
    [SerializeField] private List<Path> paths;

    public Path GetRandomPath()
    {
        int randomIndex = UnityEngine.Random.Range(0, paths.Count);

        Debug.Log(randomIndex);
        return paths[randomIndex];
    }
}

[Serializable]
public class Path
{
    public List<GameObject> PathPoints;
}