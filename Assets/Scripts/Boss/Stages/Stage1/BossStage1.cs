using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage1", menuName = "BossStage/Stage1", order = 1)]
public class BossStage1 : StageBaseSO
{
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private int numberOfProjectiles;
    [SerializeField] private float coneAngle;

    private Transform currentDestination;

    private void OnEnable()
    {
        currentDestination = null;
    }

    public override IEnumerator Attack(BossAI boss)
    {
        yield return new WaitForSeconds(delayBetweenAttacks);

        while (true)
        {
            Vector3 directionToPlayer = (boss.PlayerTransform.position - boss.BossTransform.position).normalized;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float randomAngleX = Random.Range(-coneAngle, coneAngle);
float randomAngleY = Random.Range(-coneAngle, coneAngle);
                Vector3 randomDirection = Quaternion.Euler(randomAngleX, randomAngleY, 0) * directionToPlayer;

                Vector3 spawnPosition = boss.BossTransform.position;

                GameObject instance = ProjectilePooling.Instance.GetProjectile(spawnPosition, Quaternion.identity);
                ProjectileBaseSO randomProjectile = GetRandomProjectile(boss, numberOfProjectiles, true);
                randomProjectile.CanBeMixed = false;
                instance.GetComponent<ProjectilePrefab>().SetCurrentProjectile(randomProjectile, true);

                if (instance.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.velocity = 2f * randomProjectile.Speed * randomDirection;
                }
            }

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
}

