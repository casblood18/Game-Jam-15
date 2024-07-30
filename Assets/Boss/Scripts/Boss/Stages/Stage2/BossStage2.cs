using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage1", menuName = "BossStage/Stage2", order = 1)]
public class BossStage2 : StageBaseSO
{
    [SerializeField] private float attackDelay;
    [SerializeField] private ushort projectilesAmount;

    public override IEnumerator Attack(BossAI boss)
    {
        yield return new WaitForSeconds(attackDelay);

        while (true)
        {
            yield return PerformAttack(boss, boss.RedProjectile);
            yield return new WaitForSeconds(attackDelay);

            yield return PerformAttack(boss, boss.BlueProjectile);
            yield return new WaitForSeconds(attackDelay);

            yield return PerformAttack(boss, boss.YellowProjectile);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    private IEnumerator PerformAttack(BossAI boss, ProjectileBaseSO projectile)
    {
        Vector3 center = boss.BossTransform.position;

        for (int i = 0; i < projectilesAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / projectilesAmount;

            Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));

            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, rotationAngle);

            GameObject instance = ProjectilePooling.Instance.GetProjectile(center, rotation);


            instance.GetComponent<ProjectilePrefab>().SetCurrentProjectile(projectile, false);

            if (instance.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = direction * projectile.Speed;
            }
        }
        
        yield return null;
    }

    public override void Movement(BossAI boss)
    {
        if (boss.BossTransform.position == boss.MiddlePoint.position) return;

        boss.BossTransform.position = Vector3.MoveTowards(boss.BossTransform.position, boss.MiddlePoint.position, boss.MovementSpeed * Time.deltaTime);
    }
}
