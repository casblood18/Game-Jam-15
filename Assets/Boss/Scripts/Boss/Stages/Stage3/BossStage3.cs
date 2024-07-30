using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage3", menuName = "BossStage/Stage3", order = 1)]
public class BossStage3 : StageBaseSO
{
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private int numberOfProjectiles;
    [SerializeField] private float backProjectilesSpeed;
    [Space(10)]
    [SerializeField] private ProjectileBaseSO[] projectilesAvailable;

    private Transform currentDestination;

    private void OnEnable()
    {
        currentDestination = null;
    }

    public override IEnumerator Attack(BossAI boss)
    {
        yield return new WaitForSeconds(delayBetweenAttacks / 2);

        while (true)
        {
            boss.ConeObject.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                Vector3 spawnPosition = boss.BossTransform.position;

                GameObject instance = ProjectilePooling.Instance.GetProjectile(spawnPosition, Quaternion.identity);
                ProjectileBaseSO randomProjectile = GetRandomProjectile(boss, numberOfProjectiles, true);

                Path randomPath = boss.BossPaternPaths.GetRandomPath();

                Debug.Log(randomPath.PathPoints.Count);
                instance.GetComponent<ProjectilePrefab>().SetCurrentProjectile(randomProjectile, true, randomPath);
            }

            RandomCombination(boss);

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }

    public override void Movement(BossAI boss)
    {
        if (currentDestination == null || Vector3.Distance(boss.BossTransform.position, currentDestination.position) <= 0.1f)
        {
            currentDestination = WallRunPath.Instance.MoveToNextPoint();
        }

        if (currentDestination != null)
        {
            float step = boss.MovementSpeed * Time.deltaTime;
            boss.BossTransform.position = Vector3.MoveTowards(boss.BossTransform.position, currentDestination.position, step);
        }
    }

  private void RandomCombination(BossAI boss)
{
    List<GameObject> points = WallRunPath.Instance.GetPoints();
    int totalPoints = points.Count;

    int closestIndex = GetClosestPointIndex(boss.BossTransform.position, points);

    int oppositeIndex = (closestIndex + totalPoints / 2) % totalPoints;

    Transform meetingPoint = points[oppositeIndex].transform;

    GameObject firstProjectile = ProjectilePooling.Instance.GetProjectile(boss.BossTransform.position, Quaternion.identity);
    GameObject secondProjectile = ProjectilePooling.Instance.GetProjectile(boss.BossTransform.position, Quaternion.identity);

    ProjectileBaseSO firstProjectileBase = GetRandomProjectile(boss, 3, true);
    ProjectileBaseSO secondProjectileBase = GetSecondProjectile(firstProjectileBase);

    Path clockwisePath = CreatePath(boss.BossTransform.position, meetingPoint, points, true);
    Path counterClockwisePath = CreatePath(boss.BossTransform.position, meetingPoint, points, false);

    float clockwiseDistance = CalculatePathDistance(clockwisePath);
    float counterClockwiseDistance = CalculatePathDistance(counterClockwisePath);

    float fixedTime = 5.0f;

    float clockwiseSpeed = clockwiseDistance / fixedTime;
    float counterClockwiseSpeed = counterClockwiseDistance / fixedTime;

    ushort randomSignNumber = (ushort)UnityEngine.Random.Range(0, 10000);

    firstProjectile.GetComponent<ProjectilePrefab>().SetCurrentProjectile(
        firstProjectileBase, false, clockwisePath, clockwiseSpeed * backProjectilesSpeed, false, randomSignNumber
    );

    secondProjectile.GetComponent<ProjectilePrefab>().SetCurrentProjectile(
        secondProjectileBase, false, counterClockwisePath, counterClockwiseSpeed * backProjectilesSpeed, false, randomSignNumber
    );
}

    private float CalculatePathDistance(Path path)
    {
        float distance = 0.0f;
        for (int i = 0; i < path.PathPoints.Count - 1; i++)
        {
            distance += Vector3.Distance(path.PathPoints[i].transform.position, path.PathPoints[i + 1].transform.position);
        }
        return distance;
    }

    private Path CreatePath(Vector3 startPosition, Transform meetingPoint, List<GameObject> points, bool clockwise)
    {
        Path path = new Path()
        {
            PathPoints = new List<GameObject>()
        };

        int meetingIndex = points.IndexOf(meetingPoint.gameObject);
        int closestIndex = GetClosestPointIndex(startPosition, points);

        int currentIndex = closestIndex;

        if (clockwise)
        {
            while (currentIndex != meetingIndex)
            {
                path.PathPoints.Add(points[currentIndex]);
                currentIndex = (currentIndex + 1) % points.Count;
            }
        }
        else
        {
            while (currentIndex != meetingIndex)
            {
                path.PathPoints.Add(points[currentIndex]);
                currentIndex = (currentIndex - 1 + points.Count) % points.Count;
            }
        }

        path.PathPoints.Add(points[meetingIndex]);

        return path;
    }

    private int GetClosestPointIndex(Vector3 position, List<GameObject> points)
    {
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(position, points[0].transform.position);

        for (int i = 1; i < points.Count; i++)
        {
            float distance = Vector3.Distance(position, points[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }



    private ProjectileBaseSO GetSecondProjectile(ProjectileBaseSO firstProjectile)
    {
        List<ProjectileBaseSO> secondProjectiles = new();
        switch (firstProjectile.ProjectileType)
        {
            case ProjectileEnum.Blue:
                secondProjectiles.AddRange(projectilesAvailable.Where(p => p.ProjectileType == ProjectileEnum.Red || p.ProjectileType == ProjectileEnum.Yellow));
                break;
            case ProjectileEnum.Red:
                secondProjectiles.AddRange(projectilesAvailable.Where(p => p.ProjectileType == ProjectileEnum.Blue || p.ProjectileType == ProjectileEnum.Yellow));
                break;
            case ProjectileEnum.Yellow:
                secondProjectiles.AddRange(projectilesAvailable.Where(p => p.ProjectileType == ProjectileEnum.Red || p.ProjectileType == ProjectileEnum.Blue));
                break;
            default:
                break;
        }

        return secondProjectiles[UnityEngine.Random.Range(0, secondProjectiles.Count)];
    }
}
