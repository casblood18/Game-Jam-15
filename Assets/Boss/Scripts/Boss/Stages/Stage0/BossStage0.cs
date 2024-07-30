using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage0", menuName = "BossStage/Stage0", order = 1)]
public class BossStage0 : StageBaseSO
{
    [SerializeField] private float longerAttackDelay;
    [SerializeField] private float shorterAttackDelay;
    [SerializeField] private ushort projectilesAmount;

    [Space(10)]
    [SerializeField] private ushort simpleAttackAmount = 3;

    private bool isNewWave = false;

    public override IEnumerator Attack(BossAI boss)
    {
        //first 3 "longer" waves - projectiles can't be mixed
        yield return PerformAttack(boss, longerAttackDelay, simpleAttackAmount, projectilesAmount, false);

        //then 3 "shorter" waves - projectiles can be mixed
        yield return PerformAttack(boss, shorterAttackDelay, simpleAttackAmount, projectilesAmount, true);
    }

    private IEnumerator PerformAttack(BossAI boss, float attackDelay, ushort attackAmount, int projectilesAmount, bool canBeMixed)
    {
        yield return new WaitForSeconds(attackDelay);

        while (attackAmount > 0)
        {
            Vector3 center = boss.BossTransform.position;

            isNewWave = true;

            for (int i = 0; i < projectilesAmount; i++)
            {
                float angle = i * Mathf.PI * 2 / projectilesAmount;

                float randomAngle = UnityEngine.Random.Range(0f, 360f);

                Vector2 direction = new(Mathf.Cos(angle + randomAngle), Mathf.Sin(angle + randomAngle));


                float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
                Quaternion rotation = Quaternion.Euler(0f, 0f, rotationAngle);

                GameObject instance = ProjectilePooling.Instance.GetProjectile(center, rotation);

                ProjectileBaseSO randomProjectile = GetRandomProjectile(boss, projectilesAmount, isNewWave);
                isNewWave = false;

                instance.GetComponent<ProjectilePrefab>().SetCurrentProjectile(randomProjectile, canBeMixed);

                if (instance.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.velocity = direction * randomProjectile.Speed;
                }
            }

            attackAmount--;
            yield return new WaitForSeconds(attackDelay);
        }

        yield return null;
    }

    public override void Movement(BossAI boss)
    {
        //stage 0: stay in the spot?
    }
}