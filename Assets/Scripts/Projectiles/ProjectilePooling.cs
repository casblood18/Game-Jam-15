using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePooling : MonoBehaviour
{
    public static ProjectilePooling Instance;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileLifetime;
    private ObjectPool<GameObject> projectilePool;
    private List<GameObject> activeProjectiles;

    void Awake()
    {
        Instance = this;

        activeProjectiles = new List<GameObject>();

        projectilePool = new ObjectPool<GameObject>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnedToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: false,
            defaultCapacity: 30,
            maxSize: 150
        );
    }
    void OnEnable()
    {
        BossHealth.OnBossDeath += ReleaseAllProjectiles;
    }

    void OnDisable()
    {
        BossHealth.OnBossDeath -= ReleaseAllProjectiles;
    }
    
    private GameObject CreatePooledItem()
    {
        return Instantiate(projectilePrefab);
    }

    private void OnTakeFromPool(GameObject projectile)
    {
        projectile.SetActive(true);
        activeProjectiles.Add(projectile);
    }

    private void OnReturnedToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        activeProjectiles.Remove(projectile);
    }

    private void OnDestroyPoolObject(GameObject projectile)
    {
        Destroy(projectile);
    }

    public GameObject GetProjectile(Vector3 position, Quaternion rotation)
    {
        GameObject projectile = projectilePool.Get();
        projectile.transform.SetPositionAndRotation(position, rotation);
        return projectile;
    }

    public void ReleaseProjectile(GameObject projectile)
    {
        projectilePool.Release(projectile);
    }

    public void ReleaseAllProjectiles()
    {
        foreach (GameObject projectile in new List<GameObject>(activeProjectiles))
        {
            ReleaseProjectile(projectile);
        }
    }
}
