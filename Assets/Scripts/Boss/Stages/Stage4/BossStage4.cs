using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage4", menuName = "BossStage/Stage4", order = 1)]
public class BossStage4 : StageBaseSO
{

    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private int numberOfProjectiles;

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
                instance.GetComponent<ProjectilePrefab>().SetCurrentProjectile(randomProjectile, true, randomPath);
            }

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }


    public override void Movement(BossAI boss)
    {

    }
}
