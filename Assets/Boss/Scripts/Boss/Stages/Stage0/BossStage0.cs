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
    private bool canAttack = false;
    private Transform currentDestination;

    private void OnEnable()
    {
        currentDestination = null;
        canAttack = false;
    }

    public override IEnumerator Attack(BossAI boss)
    {
        yield return new WaitUntil(() => canAttack);

        //first 3 "longer" waves - projectiles can't be mixed
        yield return PerformAttack(boss, longerAttackDelay, simpleAttackAmount, projectilesAmount, false);

        //then 3 "shorter" waves - projectiles can be mixed
        yield return PerformAttack(boss, shorterAttackDelay, simpleAttackAmount, projectilesAmount, true);
    }

    private IEnumerator PerformAttack(BossAI boss, float attackDelay, ushort attackAmount, int projectilesAmount, bool canBeMixed)
    {
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
        if (currentDestination == null || Vector3.Distance(boss.BossTransform.position, currentDestination.position) <= 0.1f)
        {
            currentDestination = WallRunPath.Instance.MoveToNextCirclePoint();
        }

        if ( !canAttack && Vector3.Distance(boss.BossTransform.position, Player.Instance.transform.position) <= 4f)
        {
            Debug.Log("can attack");
            canAttack = true;
        }

        if (currentDestination != null)
        {
            float step = boss.MovementSpeed * Time.deltaTime;
            boss.BossTransform.position = Vector3.MoveTowards(boss.BossTransform.position, currentDestination.position, step);
        }
    }
}